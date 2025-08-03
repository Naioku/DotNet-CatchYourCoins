using Application.DTOs.InputDTOs.Expenses;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Expenses.Commands.Create;
using Application.Requests.Expenses.Queries.GetById;
using Domain;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Expenses;

public class Expenses(TestFixture fixture) : TestBase(fixture)
{
    private readonly IMediator _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    private ExpenseCategory? _categoryUser1;
    private ExpensePaymentMethod? _paymentMethodUser1;
    private ExpenseCategory? _categoryUser2;
    private ExpensePaymentMethod? _paymentMethodUser2;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        _categoryUser1 = await AddCategory(new ExpenseCategory
        {
            Name = "Test1",
            Limit = 1000,
            UserId = user1.Id,
        });
        
        _categoryUser2 = await AddCategory(new ExpenseCategory
        {
            Name = "Test2",
            Limit = 2000,
            UserId = user2.Id,
        });
        
        _paymentMethodUser1 = await AddPaymentMethod(new ExpensePaymentMethod
        {
            Name = "Test1",
            Limit = 1000,
            UserId = user1.Id,
        });
        
        _paymentMethodUser2 = await AddPaymentMethod(new ExpensePaymentMethod
        {
            Name = "Test2",
            Limit = 2000,
            UserId = user2.Id,
        });
        
        await dbContext.SaveChangesAsync();
    }

    private async Task<ExpenseCategory> AddCategory(ExpenseCategory category)
        => (await dbContext.Set<ExpenseCategory>().AddAsync(category)).Entity;

    private async Task<ExpensePaymentMethod> AddPaymentMethod(ExpensePaymentMethod paymentMethod)
        => (await dbContext.Set<ExpensePaymentMethod>().AddAsync(paymentMethod)).Entity;

    [Fact]
    public async Task CreateExpense_WithValidData_ShouldCreateExpenseInDB()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
        Assert.NotNull(_paymentMethodUser1);
    
        CommandCreateExpense command = new()
        {
            Data = new InputDTOExpense
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
                CategoryId = _categoryUser1.Id,
                PaymentMethodId = _paymentMethodUser1.Id,
            }
        };
    
        // Act
        Result result = await _mediator.Send(command);
    
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        
        Expense? entity = await dbContext.Set<Expense>().FirstOrDefaultAsync();
    
        Assert.NotNull(entity);
        Assert.Equal(entity.Amount, command.Data.Amount);
        Assert.Equal(entity.Date, command.Data.Date);
        Assert.Equal(entity.Description, command.Data.Description);
        Assert.Equal(entity.CategoryId, command.Data.CategoryId);
        Assert.Equal(entity.PaymentMethodId, command.Data.PaymentMethodId);
    }
    
    [Fact]
    public async Task CreateExpense_WithInvalidCategoryIdAndPaymentMethodId_ShouldNotCreateExpenseInDB()
    {
        // Arrange
        CommandCreateExpense command = new()
        {
            Data = new InputDTOExpense
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
                CategoryId = -1,
                PaymentMethodId = -1,
            }
        };
    
        // Act
        Result result = await _mediator.Send(command);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
    }
    
    [Fact]
    public async Task CreateExpense_WithOnlyCategoryId_ShouldCreateExpenseInDBWithNullPaymentMethodId()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
        
        CommandCreateExpense command = new()
        {
            Data = new InputDTOExpense
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
                CategoryId = _categoryUser1.Id,
            }
        };
    
        // Act
        Result result = await _mediator.Send(command);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        
        Expense? entity = await dbContext.Set<Expense>().FirstOrDefaultAsync();
        Assert.NotNull(entity);
        Assert.Equal(command.Data.Amount, entity.Amount);
        Assert.Equal(command.Data.Date, entity.Date);
        Assert.Equal(command.Data.Description, entity.Description);
        Assert.Equal(command.Data.CategoryId, entity.CategoryId);
        Assert.Null(entity.PaymentMethodId);
    }
    
    [Fact]
    public async Task CreateExpense_WithOnlyPaymentMethodId_ShouldCreateExpenseInDBWithNullCategoryId()
    {
        // Arrange
        Assert.NotNull(_paymentMethodUser1);
        
        CommandCreateExpense command = new()
        {
            Data = new InputDTOExpense
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
                PaymentMethodId = _paymentMethodUser1.Id,
            }
        };
    
        // Act
        Result result = await _mediator.Send(command);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        
        Expense? entity = await dbContext.Set<Expense>().FirstOrDefaultAsync();
        Assert.NotNull(entity);
        Assert.Equal(command.Data.Amount, entity.Amount);
        Assert.Equal(command.Data.Date, entity.Date);
        Assert.Equal(command.Data.Description, entity.Description);
        Assert.Equal(command.Data.PaymentMethodId, entity.PaymentMethodId);
        Assert.Null(entity.CategoryId);
    }
    
    [Fact]
    public async Task GetExpense_WithValidData_ShouldReturnExpenseForView()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
        Assert.NotNull(_paymentMethodUser1);

        Expense expense = new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = _testServiceCurrentUser.User.Id,
            CategoryId = _categoryUser1.Id,
            PaymentMethodId = _paymentMethodUser1.Id,
        };
        await dbContext.Set<Expense>().AddAsync(expense);
        await dbContext.SaveChangesAsync();
    
        var query = new QueryGetExpenseById { Id = expense.Id };
    
        // Act
        Result<OutputDTOExpense> result = await _mediator.Send(query);
    
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);
    
        OutputDTOExpense dto = result.Value;
        Assert.NotNull(dto);
        Assert.Equal(query.Id, dto.Id);
        Assert.Equal(dto.Amount, dto.Amount);
        Assert.Equal(dto.Date, dto.Date);
        Assert.Equal(dto.Description, dto.Description);
        Assert.Equal(_categoryUser1.Name, dto.Category);
        Assert.Equal(_paymentMethodUser1.Name, dto.PaymentMethod);
    }
    
    [Fact]
    public async Task GetExpense_WithInvalidUser_ShouldReturnNull()
    {
        // Arrange
        Assert.NotNull(_categoryUser2);
        Assert.NotNull(_paymentMethodUser2);

        Expense expense = new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = user2.Id,
            CategoryId = _categoryUser2.Id,
            PaymentMethodId = _paymentMethodUser2.Id,
        };
        await dbContext.Set<Expense>().AddAsync(expense);
        await dbContext.SaveChangesAsync();
    
        var query = new QueryGetExpenseById { Id = expense.Id };
    
        // Act
        Result<OutputDTOExpense> result = await _mediator.Send(query);
    
        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Value);
    }
}