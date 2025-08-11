using Application.Dashboard.Commands;
using Application.Dashboard.DTOs.InputDTOs.Expenses;
using Domain;
using Domain.Dashboard.Entities.Expenses;
using Domain.Dashboard.Specifications.Expenses;
using Domain.Interfaces.Services;
using FluentAssertions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Dashboard.Expenses;

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
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        
        ExpensePaymentMethod? paymentMethod = await _dbContext.Set<ExpensePaymentMethod>().FirstOrDefaultAsync();
        
        paymentMethod.Should().NotBeNull();
        paymentMethod.UserId.Should().Be(_testServiceCurrentUser.User.Id);
        paymentMethod.Name.Should().Be(command.Data.Name);
        paymentMethod.Limit.Should().Be(command.Data.Limit);
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
            Specification = SpecificationExpensePaymentMethod.GetBuilder()
                .WithId(paymentMethod.Id)
                .Build(),
        };
        
        // Act
        Result result = await _mediator.Send(command);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        
        Expense? entity = await _dbContext.Set<Expense>().FirstOrDefaultAsync();

        entity.Should().NotBeNull();
        entity.PaymentMethod.Should().BeNull();
        entity.PaymentMethodId.Should().BeNull();
    }
}