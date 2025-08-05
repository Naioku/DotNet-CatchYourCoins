using Application.DTOs.InputDTOs.Expenses;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Commands;
using Application.Requests.Queries;
using Domain;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Services;
using FluentAssertions;
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

        CommandCRUDCreate<InputDTOExpense> command = new()
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
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        Expense? entity = await dbContext.Set<Expense>().FirstOrDefaultAsync();

        entity.Should().NotBeNull();
        entity.UserId.Should().Be(_testServiceCurrentUser.User.Id);
        entity.Amount.Should().Be(command.Data.Amount);
        entity.Date.Should().Be(command.Data.Date);
        entity.Description.Should().Be(command.Data.Description);
        entity.CategoryId.Should().Be(command.Data.CategoryId);
        entity.PaymentMethodId.Should().Be(command.Data.PaymentMethodId);
    }

    [Fact]
    public async Task CreateExpense_WithOnlyCategoryId_ShouldCreateExpenseInDBWithNullPaymentMethodId()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);

        CommandCRUDCreate<InputDTOExpense> command = new()
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
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        Expense? entity = await dbContext.Set<Expense>().FirstOrDefaultAsync();

        entity.Should().NotBeNull();
        entity.UserId.Should().Be(_testServiceCurrentUser.User.Id);
        entity.Amount.Should().Be(command.Data.Amount);
        entity.Date.Should().Be(command.Data.Date);
        entity.Description.Should().Be(command.Data.Description);
        entity.CategoryId.Should().Be(command.Data.CategoryId);
        entity.PaymentMethodId.Should().BeNull();
    }

    [Fact]
    public async Task CreateExpense_WithOnlyPaymentMethodId_ShouldCreateExpenseInDBWithNullCategoryId()
    {
        // Arrange
        Assert.NotNull(_paymentMethodUser1);

        CommandCRUDCreate<InputDTOExpense> command = new()
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
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        Expense? entity = await dbContext.Set<Expense>().FirstOrDefaultAsync();

        entity.Should().NotBeNull();
        entity.UserId.Should().Be(_testServiceCurrentUser.User.Id);
        entity.Amount.Should().Be(command.Data.Amount);
        entity.Date.Should().Be(command.Data.Date);
        entity.Description.Should().Be(command.Data.Description);
        entity.CategoryId.Should().BeNull();
        entity.PaymentMethodId.Should().Be(command.Data.PaymentMethodId);
    }

    [Fact]
    public async Task CreateExpense_WithInvalidCategoryIdAndPaymentMethodId_ShouldNotCreateExpenseInDB()
    {
        // Arrange
        CommandCRUDCreate<InputDTOExpense> command = new()
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
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetExpense_WithValidData_ShouldReturnExpenseForView()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
        Assert.NotNull(_paymentMethodUser1);

        Expense entity = new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = _testServiceCurrentUser.User.Id,
            CategoryId = _categoryUser1.Id,
            PaymentMethodId = _paymentMethodUser1.Id,
        };
        await dbContext.Set<Expense>().AddAsync(entity);
        await dbContext.SaveChangesAsync();

        QueryCRUDGetById<OutputDTOExpense> query = new() { Id = entity.Id };

        // Act
        Result<OutputDTOExpense> result = await _mediator.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().NotBeNull();

        OutputDTOExpense dto = result.Value;

        dto.Should().NotBeNull();
        dto.Id.Should().Be(query.Id);
        dto.Amount.Should().Be(entity.Amount);
        dto.Date.Should().Be(entity.Date);
        dto.Description.Should().Be(entity.Description);
        dto.Category.Should().Be(_categoryUser1.Name);
        dto.PaymentMethod.Should().Be(_paymentMethodUser1.Name);
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

        QueryCRUDGetById<OutputDTOExpense> query = new() { Id = expense.Id };

        // Act
        Result<OutputDTOExpense> result = await _mediator.Send(query);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Value.Should().BeNull();
    }
}