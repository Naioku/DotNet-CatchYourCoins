using Application.Expenses.Commands;
using Domain.Dashboard.Entities;
using Domain.IdentityEntities;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Expenses;

public class Categories(TestFixture fixture) : IClassFixture<TestFixture>, IAsyncLifetime
{
    private readonly IMediator _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    private readonly AppDbContext _dbContext = fixture.ServiceProvider.GetRequiredService<AppDbContext>();
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    public async Task InitializeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.EnsureCreatedAsync();

        CurrentUser currentUser = _testServiceCurrentUser.User;
        await _dbContext.Users.AddAsync(new AppUser
        {
            Id = currentUser.Id,
            UserName = currentUser.Name,
            NormalizedUserName = currentUser.Name.ToUpper(),
            Email = currentUser.Email,
            NormalizedEmail = currentUser.Email.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        });
        await _dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync() => await _dbContext.Database.EnsureDeletedAsync();

    [Fact]
    public async Task AddCategory_WithValidData_ShouldCreateCategoryInDB()
    {
        // Arrange
        var command = new CommandAddCategory
        {
            Name = "Test",
            Limit = 1000
        };

        // Act
        await _mediator.Send(command);

        // Assert
        Category? category = await _dbContext.Categories.FirstOrDefaultAsync();
        
        Assert.NotNull(category);
        Assert.Equal(category.UserId, _testServiceCurrentUser.User.Id);
        Assert.Equal(category.Name, command.Name);
        Assert.Equal(category.Limit, command.Limit);
    }
}