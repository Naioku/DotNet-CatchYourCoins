using Application.Dashboard.Commands;
using Application.Dashboard.DTOs.InputDTOs.Expenses;
using Domain;
using Domain.Dashboard.Entities.Expenses;
using Domain.Dashboard.Specifications.Expenses;
using Domain.Interfaces.Services;
using FluentAssertions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Dashboard.Expenses;

public class Categories(TestFixture fixture) : TestBase(fixture)
{
    private readonly IMediator _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    private readonly AppDbContext _dbContext = fixture.ServiceProvider.GetRequiredService<AppDbContext>();
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    [Fact]
    public async Task CreateCategory_WithValidData_ShouldCreateCategoryInDB()
    {
        // Arrange
        CommandCRUDCreate<InputDTOExpenseCategory> command = new()
        {
            Data = new InputDTOExpenseCategory
            {
                Name = "Test1",
                Limit = 1000
            },
        };

        // Act
        Result result = await _mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        ExpenseCategory? category = await _dbContext.Set<ExpenseCategory>().FirstOrDefaultAsync();

        category.Should().NotBeNull();
        category.UserId.Should().Be(_testServiceCurrentUser.User.Id);
        category.Name.Should().Be(command.Data.Name);
        category.Limit.Should().Be(command.Data.Limit);
    }

    [Fact]
    public async Task DeleteCategory_WhenCategoryBelongsToExpense_ShouldDeleteCategoryLeavingNullInExpensesDB()
    {
        // Arrange
        ExpenseCategory category = new ExpenseCategory
        {
            Name = "Test",
            Limit = 1000,
            UserId = user1.Id,
        };
        await dbContext.Set<ExpenseCategory>().AddAsync(category);
        await dbContext.SaveChangesAsync();

        await dbContext.Set<Expense>().AddAsync(new Expense
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = user1.Id,
            CategoryId = category.Id,
        });
        await dbContext.SaveChangesAsync();

        CommandCRUDDelete<ExpenseCategory> command = new()
        {
            Specification = SpecificationExpenseCategory.GetBuilder()
                .WithId(category.Id)
                .Build(),
        };

        // Act
        Result result = await _mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        Expense? entity = await _dbContext.Set<Expense>().FirstOrDefaultAsync();

        entity.Should().NotBeNull();
        entity.Category.Should().BeNull();
        entity.CategoryId.Should().BeNull();
    }
}