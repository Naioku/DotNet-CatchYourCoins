using Application.Dashboard.Commands;
using Application.Dashboard.DTOs.CreateDTOs.Incomes;
using Application.Dashboard.DTOs.UpdateDTOs.Incomes;
using Domain;
using Domain.Dashboard.Entities.Incomes;
using Domain.Dashboard.Specifications.Incomes;
using Domain.Interfaces.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Dashboard.Incomes;

public class Categories(TestFixture fixture) : TestBase(fixture)
{
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    [Fact]
    public async Task CreateCategory_WithValidData_ShouldCreateCategoryInDB()
    {
        // Arrange
        CommandCRUDCreate<CreateDTOIncomeCategory> command = new()
        {
            Data = new CreateDTOIncomeCategory
            {
                Name = "Test",
                Limit = 1000
            }
        };

        // Act
        Result result = await mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        
        IncomeCategory? category = await dbContext.Set<IncomeCategory>().FirstOrDefaultAsync();

        category.Should().NotBeNull();
        category.UserId.Should().Be(_testServiceCurrentUser.User.Id);
        category.Name.Should().Be(command.Data.Name);
        category.Limit.Should().Be(command.Data.Limit);
    }
    
    [Fact]
    public async Task DeleteCategory_WhenCategoryBelongsToIncome_ShouldCreateCategoryLeavingNullInExpensesDB()
    {
        // Arrange
        IncomeCategory category = new IncomeCategory
        {
            Name = "Test",
            Limit = 1000,
            UserId = user1.Id,
        };
        await dbContext.Set<IncomeCategory>().AddAsync(category);
        await dbContext.SaveChangesAsync();

        await dbContext.Set<Income>().AddAsync(new Income
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = user1.Id,
            CategoryId = category.Id,
        });
        await dbContext.SaveChangesAsync();
        
        CommandCRUDDelete<IncomeCategory> command = new()
        {
            Specification = SpecificationIncomeCategory.GetBuilder()
                .WithId(category.Id)
                .Build(),
        };
        
        // Act
        Result result = await mediator.Send(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        
        Income? entity = await dbContext.Set<Income>().FirstOrDefaultAsync();

        entity.Should().NotBeNull();
        entity.Category.Should().BeNull();
        entity.CategoryId.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateExpenseCategory_WithValidData_ShouldUpdateExpenseInDB()
    {
        // Arrange
        IReadOnlyList<IncomeCategory> entities =
        [
            new()
            {
                UserId = _testServiceCurrentUser.User.Id,
                Name = "Test1",
                Limit = 1000,
            }
        ];
        await dbContext.Set<IncomeCategory>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        CommandCRUDUpdate<IncomeCategory, UpdateDTOIncomeCategory> command = new()
        {
            Specification = SpecificationIncomeCategory.GetBuilder()
                .WithIdRange(entities.Select(e => e.Id).ToList())
                .Build(),
            Data =
            [
                new UpdateDTOIncomeCategory
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

        IReadOnlyList<IncomeCategory> entitiesUpdated = await dbContext.Set<IncomeCategory>()
            .ToListAsync();

        entitiesUpdated.Should().NotBeEmpty();

        for (var i = 0; i < entitiesUpdated.Count; i++)
        {
            IncomeCategory entity = entitiesUpdated[i];
            entity.UserId.Should().Be(_testServiceCurrentUser.User.Id);
            entity.Name.Should().Be(command.Data[i].Name.Value);
            entity.Limit.Should().Be(command.Data[i].Limit.Value);
        }
    }
}