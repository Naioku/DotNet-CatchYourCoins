using Application.Dashboard.Commands;
using Application.Dashboard.DTOs.CreateDTOs.Incomes;
using Domain;
using Domain.Dashboard.Entities.Incomes;
using Domain.Dashboard.Specifications.Incomes;
using Domain.Interfaces.Services;
using FluentAssertions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Dashboard.Incomes;

public class Categories(TestFixture fixture) : TestBase(fixture)
{
    private readonly IMediator _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    private readonly AppDbContext _dbContext = fixture.ServiceProvider.GetRequiredService<AppDbContext>();
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
        Result result = await _mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        
        IncomeCategory? category = await _dbContext.Set<IncomeCategory>().FirstOrDefaultAsync();

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
        Result result = await _mediator.Send(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        
        Income? entity = await _dbContext.Set<Income>().FirstOrDefaultAsync();

        entity.Should().NotBeNull();
        entity.Category.Should().BeNull();
        entity.CategoryId.Should().BeNull();
    }
}