using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Queries;

[TestSubject(typeof(HandlerGetPaymentMethodById))]
public class HandlerGetPaymentMethodByIdTest : CQRSHandlerTestBase<HandlerGetPaymentMethodById>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryPaymentMethod>();
        return base.InitializeAsync();
    }

    protected override HandlerGetPaymentMethodById CreateHandler() =>
        new(GetMock<IRepositoryPaymentMethod>().Object);

    [Fact]
    public async Task GetOne_ValidData_Return()
    {
        // Arrange
        PaymentMethod paymentMethod = FactoryPaymentMethod.CreateEntity(TestFactoryUsers.DefaultUser1Authenticated);
        GetMock<IRepositoryPaymentMethod>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == paymentMethod.Id
            )))
            .ReturnsAsync(paymentMethod);

        QueryGetPaymentMethodById query = new() { Id = paymentMethod.Id };


        // Act
        Result<PaymentMethodDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        PaymentMethodDTO paymentMethodDTO = result.Value;
        Assert.Equal(query.Id, paymentMethodDTO.Id);
        Assert.Equal(paymentMethod.Name, paymentMethodDTO.Name);
        Assert.Equal(paymentMethod.Limit, paymentMethodDTO.Limit);
    }

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnNull()
    {
        // Arrange
        QueryGetPaymentMethodById query = new() { Id = 1 };

        GetMock<IRepositoryPaymentMethod>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == query.Id
            )))
            .ReturnsAsync((PaymentMethod)null);

        // Act
        Result<PaymentMethodDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Value);
    }
}