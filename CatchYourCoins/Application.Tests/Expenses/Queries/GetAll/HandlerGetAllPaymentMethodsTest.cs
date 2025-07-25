using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries.GetAll;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetAll;

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
        List<PaymentMethod> entities = FactoryPaymentMethod.CreateEntities(TestFactoryUsers.DefaultUser1Authenticated, 5);
        GetMock<IRepositoryPaymentMethod>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync(entities);
        
        QueryGetAllPaymentMethods query = new();

        // Act
        Result<IReadOnlyList<PaymentMethodDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        // ReSharper disable once InconsistentNaming
        IReadOnlyList<PaymentMethodDTO> dtos = result.Value;

        for (var i = 0; i < dtos.Count; i++)
        {
            PaymentMethodDTO dto = dtos[i];
            Assert.Equal(entities[i].Name, dto.Name);
            Assert.Equal(entities[i].Limit, dto.Limit);
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