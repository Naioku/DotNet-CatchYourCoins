using Application.Expenses.Commands;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Expenses;

public class PaymentMethods(TestFixture fixture) : TestBase(fixture)
{
    private readonly IMediator _mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    private readonly AppDbContext _dbContext = fixture.ServiceProvider.GetRequiredService<AppDbContext>();
    private readonly IServiceCurrentUser _testServiceCurrentUser = fixture.ServiceProvider.GetRequiredService<IServiceCurrentUser>();

    [Fact]
    public async Task AddPaymentMethod_WithValidData_ShouldCreatePaymentMethodInDB()
    {
        // Arrange
        var command = new CommandAddPaymentMethod
        {
            Name = "Test",
            Limit = 1000
        };

        // Act
        await _mediator.Send(command);

        // Assert
        PaymentMethod? paymentMethod = await _dbContext.PaymentMethods.FirstOrDefaultAsync();
        
        Assert.NotNull(paymentMethod);
        Assert.Equal(paymentMethod.UserId, _testServiceCurrentUser.User.Id);
        Assert.Equal(paymentMethod.Name, command.Name);
        Assert.Equal(paymentMethod.Limit, command.Limit);
    }
}