using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.DeletePaymentMethod;

[TestSubject(typeof(HandlerDeleteCategory))]
public class HandlerDeletePaymentMethodTest : CQRSHandlerTestBase<HandlerDeletePaymentMethod>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryPaymentMethod>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected override HandlerDeletePaymentMethod CreateHandler()
    {
        return new HandlerDeletePaymentMethod(
            GetMock<IRepositoryPaymentMethod>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }

    [Fact]
    public async Task Delete_ValidData_EntryDeleted()
    {
        // Arrange
        PaymentMethod paymentMethod = FactoryPaymentMethod.CreateEntity(TestFactoryUsers.DefaultUser1Authenticated);
        GetMock<IRepositoryPaymentMethod>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == paymentMethod.Id
            )))
            .ReturnsAsync(paymentMethod);

        CommandDeletePaymentMethod command = new() { Id = paymentMethod.Id };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        GetMock<IRepositoryPaymentMethod>().Verify(m => m.Delete(It.Is<PaymentMethod>(e => e.Id == command.Id)));
        GetMock<IUnitOfWork>().Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_NoEntryInDBAtPassedID_EntryNotDeleted()
    {
        // Arrange
        GetMock<IRepositoryPaymentMethod>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == 1
            )))
            .ReturnsAsync((PaymentMethod)null);

        CommandDeletePaymentMethod command = new() { Id = 1 };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        GetMock<IRepositoryPaymentMethod>().Verify(m => m.Delete(It.IsAny<PaymentMethod>()), Times.Never);
    }
}