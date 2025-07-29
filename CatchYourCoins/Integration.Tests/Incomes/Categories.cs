using Application.Incomes.Commands.Create;
using Domain.Dashboard.Entities;
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
        var command = new CommandCreateCategory
        {
            Name = "Test",
            Limit = 1000
        };

        // Act
        await _mediator.Send(command);

        // Assert
        CategoryIncomes? category = await _dbContext.Set<CategoryIncomes>().FirstOrDefaultAsync();

        Assert.NotNull(category);
        Assert.Equal(category.UserId, _testServiceCurrentUser.User.Id);
        Assert.Equal(category.Name, command.Name);
        Assert.Equal(category.Limit, command.Limit);
    }
}