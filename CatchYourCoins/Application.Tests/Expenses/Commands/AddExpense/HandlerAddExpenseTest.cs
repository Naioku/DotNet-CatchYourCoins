using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
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

        var currentUser = new CurrentUser
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            Email = "test@example.com",
            Name = "Test User",
            IsAuthenticated = true
        };

        var repository = new Mock<IRepositoryExpense>();
        var serviceCurrentUser = new Mock<IServiceCurrentUser>();
        serviceCurrentUser.Setup(m => m.User).Returns(currentUser);

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
                e.UserId == currentUser.Id &&
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

        var currentUser = new CurrentUser
        {
            Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            Email = "test@example.com",
            Name = "Test User",
            IsAuthenticated = true
        };

        var repository = new Mock<IRepositoryExpense>();
        var serviceCurrentUser = new Mock<IServiceCurrentUser>();
        serviceCurrentUser.Setup(m => m.User).Returns(currentUser);

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
                e.UserId == currentUser.Id &&
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