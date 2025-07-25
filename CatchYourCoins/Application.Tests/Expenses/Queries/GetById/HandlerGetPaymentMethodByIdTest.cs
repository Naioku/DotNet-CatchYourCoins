using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries.GetById;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetById;

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
        PaymentMethod entity = FactoryPaymentMethod.CreateEntity(TestFactoryUsers.DefaultUser1Authenticated);
        GetMock<IRepositoryPaymentMethod>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == entity.Id
            )))
            .ReturnsAsync(entity);

        QueryGetPaymentMethodById query = new() { Id = entity.Id };


        // Act
        Result<PaymentMethodDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        PaymentMethodDTO dto = result.Value;
        Assert.Equal(query.Id, dto.Id);
        Assert.Equal(entity.Name, dto.Name);
        Assert.Equal(entity.Limit, dto.Limit);
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