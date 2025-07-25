using System.Collections.Generic;
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

[TestSubject(typeof(HandlerGetAllPaymentMethods))]
public class HandlerGetAllPaymentMethodsTest : CQRSHandlerTestBase<HandlerGetAllPaymentMethods>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryPaymentMethod>();
        return base.InitializeAsync();
    }

    protected override HandlerGetAllPaymentMethods CreateHandler() =>
        new(GetMock<IRepositoryPaymentMethod>().Object);

    
    [Fact]
    public async Task GetAll_ValidData_ReturnsAll()
    {
        // Arrange
        List<PaymentMethod> paymentMethods = TestFactoryPaymentMethod.CreatePaymentMethods(TestFactoryUsers.DefaultUser1Authenticated, 5);
        GetMock<IRepositoryPaymentMethod>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync(paymentMethods);
        
        QueryGetAllPaymentMethods query = new();

        // Act
        Result<IReadOnlyList<PaymentMethodDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        // ReSharper disable once InconsistentNaming
        IReadOnlyList<PaymentMethodDTO> paymentMethodDTOs = result.Value;

        for (var i = 0; i < paymentMethodDTOs.Count; i++)
        {
            var dto = paymentMethodDTOs[i];
            Assert.Equal(paymentMethods[i].Name, dto.Name);
            Assert.Equal(paymentMethods[i].Limit, dto.Limit);
        }
    }
    
    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnsNull()
    {
        // Arrange
        GetMock<IRepositoryPaymentMethod>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync([]);
        
        QueryGetAllPaymentMethods query = new();

        // Act
        Result<IReadOnlyList<PaymentMethodDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Value);
    }
}