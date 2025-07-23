using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddExpense;

[TestSubject(typeof(HandlerAddExpense))]
public class HandlerAddExpenseTest
{
    [Fact]
    public async Task AddExpense_AllValidData_CreateExpense()
    {
        // Arrange
        var command = new CommandAddExpense
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = 1,
            PaymentMethodId = 1
        };

        var repository = new Mock<IRepositoryExpense>();
        var serviceCurrentUser = TestFactoryUsers.MockServiceCurrentUser();

        var unitOfWork = new Mock<IUnitOfWork>();
        HandlerAddExpense handler = new(
            repository.Object,
            serviceCurrentUser.Object,
            unitOfWork.Object
        );

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        repository.Verify(
            m => m.CreateExpenseAsync(It.Is<Expense>(e =>
                e.Amount == command.Amount &&
                e.Date == command.Date &&
                e.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id &&
                e.Description == command.Description &&
                e.CategoryId == command.CategoryId &&
                e.PaymentMethodId == command.PaymentMethodId)),
            Times.Once
        );
        
        unitOfWork.Verify(m => m.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task AddExpense_MinimumValidData_CreateExpense()
    {
        // Arrange
        var command = new CommandAddExpense
        {
            Amount = 100,
            Date = DateTime.Now,
        };
        
        var repository = new Mock<IRepositoryExpense>();
        var serviceCurrentUser = TestFactoryUsers.MockServiceCurrentUser();

        var unitOfWork = new Mock<IUnitOfWork>();
        HandlerAddExpense handler = new(
            repository.Object,
            serviceCurrentUser.Object,
            unitOfWork.Object
        );

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        repository.Verify(
            m => m.CreateExpenseAsync(It.Is<Expense>(e =>
                e.Amount == command.Amount &&
                e.Date == command.Date &&
                e.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id &&
                e.Description == command.Description &&
                e.CategoryId == command.CategoryId &&
                e.PaymentMethodId == command.PaymentMethodId)),
            Times.Once
        );

        unitOfWork.Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}