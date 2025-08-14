using Application.Dashboard.Commands;
using Application.Dashboard.DTOs.CreateDTOs.Expenses;
using Application.Dashboard.DTOs.UpdateDTOs;
using Application.Dashboard.DTOs.UpdateDTOs.Expenses;
using Domain;
using Domain.Dashboard.Entities.Expenses;
using Domain.Dashboard.Specifications.Expenses;
using Domain.Interfaces.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Dashboard.Expenses;

public class Categories(TestFixture fixture) : TestBase(fixture)
{
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    [Fact]
    public async Task CreateCategory_WithValidData_ShouldCreateCategoryInDB()
    {
        // Arrange
        CommandCRUDCreate<CreateDTOExpenseCategory> command = new()
        {
            Data = new CreateDTOExpenseCategory
            {
                Name = "Test1",
                Limit = 1000
            },
        };

        // Act
        Result result = await mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        ExpenseCategory? category = await dbContext.Set<ExpenseCategory>().FirstOrDefaultAsync();

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
        Result result = await mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        Expense? entity = await dbContext.Set<Expense>().FirstOrDefaultAsync();

        entity.Should().NotBeNull();
        entity.Category.Should().BeNull();
        entity.CategoryId.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateExpenseCategory_WithValidData_ShouldUpdateExpenseInDB()
    {
        // Arrange
        IReadOnlyList<ExpenseCategory> entities =
        [
            new()
            {
                UserId = _testServiceCurrentUser.User.Id,
                Name = "Test1",
                Limit = 1000,
            }
        ];
        await dbContext.Set<ExpenseCategory>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        CommandCRUDUpdate<ExpenseCategory, UpdateDTOExpenseCategory> command = new()
        {
            Specification = SpecificationExpenseCategory.GetBuilder()
                .WithIdRange(entities.Select(e => e.Id).ToList())
                .Build(),
            Data =
            [
                new UpdateDTOExpenseCategory
                {
                    Id = entities[0].Id,
                    SetName = "Test2",
                    SetLimit = 2000,
                }
            ]
        };

        // Act
        Result result = await mediator.Send(command);
        dbContext.ChangeTracker.Clear();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();

        IReadOnlyList<ExpenseCategory> entitiesUpdated = await dbContext.Set<ExpenseCategory>()
            .ToListAsync();

        entitiesUpdated.Should().NotBeEmpty();

        for (var i = 0; i < entitiesUpdated.Count; i++)
        {
            ExpenseCategory entity = entitiesUpdated[i];
            entity.UserId.Should().Be(_testServiceCurrentUser.User.Id);
            entity.Name.Should().Be(command.Data[i].Name.Value);
            entity.Limit.Should().Be(command.Data[i].Limit.Value);
        }
    }
}