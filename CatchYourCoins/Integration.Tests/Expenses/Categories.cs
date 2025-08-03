using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Expenses.Commands.Create;
using Application.Requests.Expenses.Commands.Delete;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Expenses;

public class Categories(TestFixture fixture) : TestBase(fixture)
{
    private readonly IMediator _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    private readonly AppDbContext _dbContext = fixture.ServiceProvider.GetRequiredService<AppDbContext>();
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    [Fact]
    public async Task CreateCategory_WithValidData_ShouldCreateCategoryInDB()
    {
        // Arrange
        CommandCreateCategory command = new()
        {
            Data = new InputDTOExpenseCategory
            {
                Name = "Test1",
                Limit = 1000
            },
        };

        // Act
        await _mediator.Send(command);

        // Assert
        ExpenseCategory? category = await _dbContext.Set<ExpenseCategory>().FirstOrDefaultAsync();
        
        Assert.NotNull(category);
        Assert.Equal(category.UserId, _testServiceCurrentUser.User.Id);
        Assert.Equal(category.Name, command.Data.Name);
        Assert.Equal(category.Limit, command.Data.Limit);
    }
    
    [Fact]
    public async Task DeleteCategory_WhenCategoryBelongsToExpense_ShouldCreateCategoryLeavingNullInExpensesDB()
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
        
        CommandDeleteCategory command = new()
        {
            Id = category.Id,
        };
        
        // Act
        await _mediator.Send(command);
        
        // Assert
        Expense? entity = await _dbContext.Set<Expense>().FirstOrDefaultAsync();

        Assert.NotNull(entity);
        Assert.Null(entity.CategoryId);
        Assert.Null(entity.Category);
    }
}