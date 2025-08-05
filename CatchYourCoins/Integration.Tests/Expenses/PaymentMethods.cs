using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Commands;
using Domain;
using Domain.Dashboard.Entities.Expenses;
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
    public async Task CreatePaymentMethod_WithValidData_ShouldCreatePaymentMethodInDB()
    {
        // Arrange
        var command = new CommandCRUDCreate<InputDTOExpensePaymentMethod>
        {
            Data = new InputDTOExpensePaymentMethod
            {
                Name = "Test",
                Limit = 1000
            }
        };

        // Act
        Result result = await _mediator.Send(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        
        ExpensePaymentMethod? paymentMethod = await _dbContext.Set<ExpensePaymentMethod>().FirstOrDefaultAsync();
        
        Assert.NotNull(paymentMethod);
        Assert.Equal(paymentMethod.UserId, _testServiceCurrentUser.User.Id);
        Assert.Equal(paymentMethod.Name, command.Data.Name);
        Assert.Equal(paymentMethod.Limit, command.Data.Limit);
    }
    
    [Fact]
    public async Task DeletePaymentMethod_WhenPaymentMethodBelongsToExpense_ShouldDeletePaymentMethodLeavingNullInExpensesDB()
    {
        // Arrange
        ExpensePaymentMethod paymentMethod = new ExpensePaymentMethod
        {
            Name = "Test",
            Limit = 1000,
            UserId = user1.Id,
        };
        await dbContext.Set<ExpensePaymentMethod>().AddAsync(paymentMethod);
        await dbContext.SaveChangesAsync();

        await dbContext.Set<Expense>().AddAsync(new Expense
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = user1.Id,
            PaymentMethodId = paymentMethod.Id,
        });
        await dbContext.SaveChangesAsync();
        
        CommandCRUDDelete<ExpensePaymentMethod> command = new()
        {
            Id = paymentMethod.Id,
        };
        
        // Act
        Result result = await _mediator.Send(command);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        
        Expense? entity = await _dbContext.Set<Expense>().FirstOrDefaultAsync();

        Assert.NotNull(entity);
        Assert.Null(entity.PaymentMethod);
        Assert.Null(entity.PaymentMethodId);
    }
}