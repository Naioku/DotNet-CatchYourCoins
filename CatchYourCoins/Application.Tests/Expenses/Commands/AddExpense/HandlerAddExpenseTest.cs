using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddExpense;

[TestSubject(typeof(HandlerAddExpense))]
public class HandlerAddExpenseTest : IAsyncLifetime
{
    private Mock<IRepositoryExpense> _mockRepository;
    private Mock<IServiceCurrentUser> _mockServiceCurrentUser;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private HandlerAddExpense _handler;
    
    public Task InitializeAsync()
    {
        _mockRepository = new Mock<IRepositoryExpense>();
        _mockServiceCurrentUser = TestFactoryUsers.MockServiceCurrentUser();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        
        _handler = new HandlerAddExpense(
            _mockRepository.Object,
            _mockServiceCurrentUser.Object,
            _mockUnitOfWork.Object
        );
        
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _mockRepository = null;
        _mockServiceCurrentUser = null;
        _mockUnitOfWork = null;
        _handler = null;
        return Task.CompletedTask;
    }

    [Fact]
    public async Task AddExpense_AllValidData_CreateExpense()
    {
        // Arrange
        CommandAddExpense command = new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = 1,
            PaymentMethodId = 1
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(
            m => m.CreateExpenseAsync(It.Is<Expense>(e =>
                e.Amount == command.Amount &&
                e.Date == command.Date &&
                e.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id &&
                e.Description == command.Description &&
                e.CategoryId == command.CategoryId &&
                e.PaymentMethodId == command.PaymentMethodId)),
            Times.Once
        );

        _mockUnitOfWork.Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task AddExpense_MinimumValidData_CreateExpense()
    {
        // Arrange
        CommandAddExpense command = new()
        {
            Amount = 100,
            Date = DateTime.Now,
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(
            m => m.CreateExpenseAsync(It.Is<Expense>(e =>
                e.Amount == command.Amount &&
                e.Date == command.Date &&
                e.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id &&
                e.Description == command.Description &&
                e.CategoryId == command.CategoryId &&
                e.PaymentMethodId == command.PaymentMethodId)),
            Times.Once
        );

        _mockUnitOfWork.Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}