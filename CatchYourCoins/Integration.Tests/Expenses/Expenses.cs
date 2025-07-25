using Application.DTOs.Expenses;
using Application.Expenses.Commands;
using Application.Expenses.Queries;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Expenses;

public class Expenses(TestFixture fixture) : TestBase(fixture)
{
    private readonly IMediator _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    private Category? _categoryUser1;
    private PaymentMethod? _paymentMethodUser1;
    private Category? _categoryUser2;
    private PaymentMethod? _paymentMethodUser2;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        _categoryUser1 = await AddCategory(new Category
        {
            Name = "Test1",
            Limit = 1000,
            UserId = user1.Id,
        });
        
        _categoryUser2 = await AddCategory(new Category
        {
            Name = "Test2",
            Limit = 2000,
            UserId = user2.Id,
        });
        
        _paymentMethodUser1 = await AddPaymentMethod(new PaymentMethod
        {
            Name = "Test1",
            Limit = 1000,
            UserId = user1.Id,
        });
        
        _paymentMethodUser2 = await AddPaymentMethod(new PaymentMethod
        {
            Name = "Test2",
            Limit = 2000,
            UserId = user2.Id,
        });
        
        await dbContext.SaveChangesAsync();
    }

    private async Task<Category> AddCategory(Category category)
        => (await dbContext.Categories.AddAsync(category)).Entity;

    private async Task<PaymentMethod> AddPaymentMethod(PaymentMethod paymentMethod)
        => (await dbContext.PaymentMethods.AddAsync(paymentMethod)).Entity;

    [Fact]
    public async Task CreateExpense_WithValidData_ShouldCreateExpenseInDB()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
        Assert.NotNull(_paymentMethodUser1);
    
        var command = new CommandCreateExpense
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = _categoryUser1.Id,
            PaymentMethodId = _paymentMethodUser1.Id,
        };
    
        // Act
        await _mediator.Send(command);
    
        // Assert
        Expense? result = await dbContext.Expenses.FirstOrDefaultAsync();
    
        Assert.NotNull(result);
        Assert.Equal(result.Amount, command.Amount);
        Assert.Equal(result.Date, command.Date);
        Assert.Equal(result.Description, command.Description);
        Assert.Equal(result.CategoryId, command.CategoryId);
        Assert.Equal(result.PaymentMethodId, command.PaymentMethodId);
    }
    
    [Fact]
    public async Task GetExpense_WithValidData_ShouldReturnExpenseForView()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
        Assert.NotNull(_paymentMethodUser1);
    
        Expense expenseDB = (await dbContext.Expenses.AddAsync(new Expense
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = _testServiceCurrentUser.User.Id,
            CategoryId = _categoryUser1.Id,
            PaymentMethodId = _paymentMethodUser1.Id,
        })).Entity;
        
        await dbContext.SaveChangesAsync();
    
        var query = new QueryGetExpenseById { Id = expenseDB.Id };
    
        // Act
        Result<ExpenseDTO> result = await _mediator.Send(query);
    
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);
    
        ExpenseDTO expense = result.Value;
        Assert.NotNull(expense);
        Assert.Equal(expense.Id, query.Id);
        Assert.Equal(expense.Amount, expense.Amount);
        Assert.Equal(expense.Date, expense.Date);
        Assert.Equal(expense.Description, expense.Description);
        Assert.Equal(expense.Category, _categoryUser1.Name);
        Assert.Equal(expense.PaymentMethod, _paymentMethodUser1.Name);
    }
    
    [Fact]
    public async Task GetExpense_WithInvalidUser_ShouldReturnNull()
    {
        // Arrange
        Assert.NotNull(_categoryUser2);
        Assert.NotNull(_paymentMethodUser2);
    
        Expense expenseDB = (await dbContext.Expenses.AddAsync(new Expense
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = user2.Id,
            CategoryId = _categoryUser2.Id,
            PaymentMethodId = _paymentMethodUser2.Id,
        })).Entity;
        
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();
    
        var query = new QueryGetExpenseById { Id = expenseDB.Id };
    
        // Act
        Result<ExpenseDTO> result = await _mediator.Send(query);
    
        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Value);
    }
}