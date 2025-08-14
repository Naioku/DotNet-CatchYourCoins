using Application.Dashboard.Commands;
using Application.Dashboard.DTOs.CreateDTOs.Expenses;
using Application.Dashboard.DTOs.OutputDTOs.Expenses;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.Dashboard.DTOs.UpdateDTOs.Expenses;
using Application.Dashboard.Queries;
using Domain;
using Domain.Dashboard.Entities.Expenses;
using Domain.Dashboard.Specifications.Expenses;
using Domain.Interfaces.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Dashboard.Expenses;

public class Expenses(TestFixture fixture) : TestBase(fixture)
{
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

        CommandCRUDCreate<CreateDTOExpense> command = new()
        {
            Data = new CreateDTOExpense
            {
                Amount = 100,
                Date = DateTime.Today,
                Description = "Test",
                CategoryId = _categoryUser1.Id,
                PaymentMethodId = _paymentMethodUser1.Id,
            }
        };

        // Act
        Result result = await mediator.Send(command);
        dbContext.ChangeTracker.Clear();

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

        CommandCRUDCreate<CreateDTOExpense> command = new()
        {
            Data = new CreateDTOExpense
            {
                Amount = 100,
                Date = DateTime.Today,
                Description = "Test",
                CategoryId = _categoryUser1.Id,
            }
        };

        // Act
        Result result = await mediator.Send(command);
        dbContext.ChangeTracker.Clear();

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

        CommandCRUDCreate<CreateDTOExpense> command = new()
        {
            Data = new CreateDTOExpense
            {
                Amount = 100,
                Date = DateTime.Today,
                Description = "Test",
                PaymentMethodId = _paymentMethodUser1.Id,
            }
        };

        // Act
        Result result = await mediator.Send(command);
        dbContext.ChangeTracker.Clear();

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
        CommandCRUDCreate<CreateDTOExpense> command = new()
        {
            Data = new CreateDTOExpense
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
                CategoryId = -1,
                PaymentMethodId = -1,
            }
        };

        // Act
        Result result = await mediator.Send(command);

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
            Date = DateTime.Today,
            Description = "Test",
            UserId = _testServiceCurrentUser.User.Id,
            CategoryId = _categoryUser1.Id,
            PaymentMethodId = _paymentMethodUser1.Id,
        };
        await dbContext.Set<Expense>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        QueryCRUDGet<Expense, OutputDTOExpense> query = new()
        {
            Specification = SpecificationExpense.GetBuilder()
                .WithId(entity.Id)
                .Build(),
        };

        // Act
        Result<IReadOnlyList<OutputDTOExpense>> result = await mediator.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().NotBeNull();

        OutputDTOExpense dto = result.Value[0];

        dto.Should().NotBeNull();
        dto.Id.Should().Be(entity.Id);
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
        dbContext.ChangeTracker.Clear();

        QueryCRUDGet<Expense, OutputDTOExpense> query = new()
        {
            Specification = SpecificationExpense.GetBuilder()
                .WithId(expense.Id)
                .Build(),
        };

        // Act
        Result<IReadOnlyList<OutputDTOExpense>> result = await mediator.Send(query);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task UpdateExpense_WithValidData_ShouldUpdateExpenseInDB()
    {
        // Arrange
        Assert.NotNull(_categoryUser1);
        Assert.NotNull(_paymentMethodUser1);
        IReadOnlyList<Expense> entities =
        [
            new()
            {
                Amount = 100,
                Date = DateTime.Today,
                Description = "Test",
                UserId = _testServiceCurrentUser.User.Id,
                CategoryId = _categoryUser1.Id,
                PaymentMethodId = _paymentMethodUser1.Id,
            }
        ];
        await dbContext.Set<Expense>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        CommandCRUDUpdate<Expense, UpdateDTOExpense> command = new()
        {
            Specification = SpecificationExpense.GetBuilder()
                .WithIdRange(entities.Select(e => e.Id).ToList())
                .Build(),
            Data =
            [
                new UpdateDTOExpense
                {
                    Id = entities[0].Id,
                    Amount = new Optional<decimal>(200),
                    Description = new Optional<string?>("Test2"),
                    PaymentMethodId = new Optional<int?>(_paymentMethodUser1.Id),
                }
            ]
        };

        // Act
        Result result = await mediator.Send(command);
        dbContext.ChangeTracker.Clear();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        IReadOnlyList<Expense> entitiesUpdated = await dbContext.Set<Expense>()
            .Include(e => e.Category)
            .Include(e => e.PaymentMethod)
            .ToListAsync();

        entitiesUpdated.Should().NotBeEmpty();

        for (var i = 0; i < entitiesUpdated.Count; i++)
        {
            Expense expense = entitiesUpdated[i];
            expense.UserId.Should().Be(_testServiceCurrentUser.User.Id);
            expense.Amount.Should().Be(command.Data[i].Amount.Value);
            expense.Date.Should().Be(entities[i].Date);
            expense.Description.Should().Be(command.Data[i].Description.Value);
            expense.CategoryId.Should().Be(_categoryUser1.Id);
            expense.Category.Should().NotBeNull();
            expense.PaymentMethodId.Should().Be(_paymentMethodUser1.Id);
            expense.PaymentMethod.Should().NotBeNull();
        }
    }
}