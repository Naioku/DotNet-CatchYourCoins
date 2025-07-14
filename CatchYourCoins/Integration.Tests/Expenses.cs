using Application.Expenses.Commands;
using Domain.Dashboard.Entities;
using Domain.IdentityEntities;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public class Expenses(TestFixture fixture) : IClassFixture<TestFixture>, IAsyncLifetime
{
    private readonly IMediator _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    private readonly AppDbContext _dbContext = fixture.ServiceProvider.GetRequiredService<AppDbContext>();
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    public async Task InitializeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.EnsureCreatedAsync();

        CurrentUser currentUser = _testServiceCurrentUser.User;
        var testUser = new AppUser
        {
            Id = currentUser.Id,
            UserName = currentUser.Name,
            NormalizedUserName = currentUser.Name.ToUpper(),
            Email = currentUser.Email,
            NormalizedEmail = currentUser.Email.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        await _dbContext.Users.AddAsync(testUser);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync() => await _dbContext.Database.EnsureDeletedAsync();

    [Fact]
    public async Task AddCategory_WithValidData_ShouldCreateCategoryInDB()
    {
        // Arrange
        string name = "Test";
        decimal limit = 1000;
        var command = new CommandAddCategory
        {
            Name = name,
            Limit = limit
        };

        // Act
        await _mediator.Send(command);

        // Assert
        Category? category = await _dbContext.Categories.FirstOrDefaultAsync(
            c => c.Name == name && c.UserId == _testServiceCurrentUser.User.Id
        );
        
        Assert.NotNull(category);
        Assert.Equal(limit, category.Limit);
    }
    
    [Fact]
    public async Task AddPaymentMethod_WithValidData_ShouldCreatePaymentMethodInDB()
    {
        // Arrange
        string name = "Test";
        decimal limit = 1000;
        var command = new CommandAddPaymentMethod
        {
            Name = name,
            Limit = limit
        };

        // Act
        await _mediator.Send(command);

        // Assert
        PaymentMethod? category = await _dbContext.PaymentMethods.FirstOrDefaultAsync(
            c => c.Name == name && c.UserId == _testServiceCurrentUser.User.Id
        );
        
        Assert.NotNull(category);
        Assert.Equal(limit, category.Limit);
    }
}