using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries;
using Application.Tests.Factories;
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
    public async Task GetPaymentMethod_ValidData_ReturnPaymentMethod()
    {
        // Arrange
        QueryGetPaymentMethodById query = new() { Id = 1 };

        PaymentMethod paymentMethod = new PaymentMethod
        {
            Id = query.Id,
            Limit = 100,
            Name = "Test",
            UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
        };
        GetMock<IRepositoryPaymentMethod>()
            .Setup(m => m.GetCategoryByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(paymentMethod);

        // Act
        Result<PaymentMethodDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        PaymentMethodDTO paymentMethodDTO = result.Value;
        Assert.Equal(query.Id, paymentMethodDTO.Id);
        Assert.Equal(paymentMethod.Name, paymentMethodDTO.Name);
        Assert.Equal(paymentMethod.Limit, paymentMethodDTO.Limit);
    }
    
    [Fact]
    public async Task GetPaymentMethod_NoPaymentMethod_ReturnNull()
    {
        // Arrange
        QueryGetPaymentMethodById query = new() { Id = 1 };

        GetMock<IRepositoryPaymentMethod>()
            .Setup(m => m.GetCategoryByIdAsync(It.Is<int>(
                id => id == query.Id
            )))
            .ReturnsAsync((PaymentMethod)null);

        // Act
        Result<PaymentMethodDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);

        PaymentMethodDTO paymentMethodDTO = result.Value;
        Assert.Null(paymentMethodDTO);
    }
}