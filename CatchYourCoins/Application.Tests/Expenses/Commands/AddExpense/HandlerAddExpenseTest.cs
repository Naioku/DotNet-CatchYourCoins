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
public class HandlerAddExpenseTest : CQRSHandlerTestBase<HandlerAddExpense>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryExpense>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected override HandlerAddExpense CreateHandler()
    {
        return new HandlerAddExpense(
            GetMock<IRepositoryExpense>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
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
        await Handler.Handle(command, CancellationToken.None);

        // Assert
        GetMock<IRepositoryExpense>().Verify(
            m => m.CreateExpenseAsync(It.Is<Expense>(e =>
                e.Amount == command.Amount &&
                e.Date == command.Date &&
                e.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id &&
                e.Description == command.Description &&
                e.CategoryId == command.CategoryId &&
                e.PaymentMethodId == command.PaymentMethodId)),
            Times.Once
        );

        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}