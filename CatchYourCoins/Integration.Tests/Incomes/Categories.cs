using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Commands;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Incomes;

public class Categories(TestFixture fixture) : TestBase(fixture)
{
    private readonly IMediator _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    private readonly AppDbContext _dbContext = fixture.ServiceProvider.GetRequiredService<AppDbContext>();
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    [Fact]
    public async Task CreateCategory_WithValidData_ShouldCreateCategoryInDB()
    {
        // Arrange
        CommandCRUDCreate<InputDTOIncomeCategory> command = new()
        {
            Data = new InputDTOIncomeCategory
            {
                Name = "Test",
                Limit = 1000
            }
        };

        // Act
        await _mediator.Send(command);

        // Assert
        IncomeCategory? category = await _dbContext.Set<IncomeCategory>().FirstOrDefaultAsync();

        Assert.NotNull(category);
        Assert.Equal(category.UserId, _testServiceCurrentUser.User.Id);
        Assert.Equal(category.Name, command.Data.Name);
        Assert.Equal(category.Limit, command.Data.Limit);
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
            Id = category.Id,
        };
        
        // Act
        await _mediator.Send(command);
        
        // Assert
        Income? entity = await _dbContext.Set<Income>().FirstOrDefaultAsync();

        Assert.NotNull(entity);
        Assert.Null(entity.CategoryId);
        Assert.Null(entity.Category);
    }
}