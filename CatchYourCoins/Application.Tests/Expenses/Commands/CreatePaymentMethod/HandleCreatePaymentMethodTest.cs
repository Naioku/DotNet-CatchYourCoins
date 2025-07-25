using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.CreatePaymentMethod;

[TestSubject(typeof(HandlerCreatePaymentMethod))]
public class HandlerCreatePaymentMethodTest : CQRSHandlerTestBase<HandlerCreatePaymentMethod>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryPaymentMethod>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected override HandlerCreatePaymentMethod CreateHandler()
    {
        return new HandlerCreatePaymentMethod(
            GetMock<IRepositoryPaymentMethod>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    [Fact]
    public async Task Create_ValidData_EntryCreated()
    {
        // Arrange
        CommandCreatePaymentMethod command = new()
        {
            Name = "Test",
            Limit = 1000
        };

        // Act
        await Handler.Handle(command, CancellationToken.None);

        // Assert
        GetMock<IRepositoryPaymentMethod>().Verify(
            m => m.CreateAsync(
                It.Is<PaymentMethod>(pm =>
                    pm.Name == command.Name &&
                    pm.Limit == command.Limit &&
                    pm.UserId == TestFactoryUsers.DefaultUser1Authenticated.Id)),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}