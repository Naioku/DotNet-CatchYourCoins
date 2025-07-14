using Application.Expenses.Commands;
using Domain.Dashboard.Entities;
using Domain.IdentityEntities;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Expenses;

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
        
        await _dbContext.Categories.AddAsync(new Category
        {
            Name = "Test",
            Limit = 1000,
            UserId = _testServiceCurrentUser.User.Id,
        });
        
        await _dbContext.PaymentMethods.AddAsync(new PaymentMethod
        {
            Name = "Test",
            Limit = 1000,
            UserId = _testServiceCurrentUser.User.Id,
        });
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync() => await _dbContext.Database.EnsureDeletedAsync();

    [Fact]
    public async Task AddExpense_WithValidData_ShouldCreateExpenseInDB()
    {
        // Arrange
        var category = await _dbContext.Categories.FirstOrDefaultAsync();
        var paymentMethod = await _dbContext.PaymentMethods.FirstOrDefaultAsync();
        
        Assert.NotNull(category);
        Assert.NotNull(paymentMethod);
        
        var command = new CommandAddExpense
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = category.Id,
            PaymentMethodId = paymentMethod.Id,
        };

        // Act
        await _mediator.Send(command);

        // Assert
        Expense? expense = await _dbContext.Expenses.FirstOrDefaultAsync();
        
        Assert.NotNull(expense);
        Assert.Equal(expense.Amount, command.Amount);
        Assert.Equal(expense.Date, command.Date);
        Assert.Equal(expense.Description, command.Description);
        Assert.Equal(expense.CategoryId, command.CategoryId);
        Assert.Equal(expense.PaymentMethodId, command.PaymentMethodId);
    }
}