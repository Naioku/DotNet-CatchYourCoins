using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries.GetById;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetById;

[TestSubject(typeof(Application.Expenses.Queries.GetById.TestHandlerGetPaymentMethodById))]
public class TestHandlerGetPaymentMethodById
    : TestHandlerGetById<
        Application.Expenses.Queries.GetById.TestHandlerGetPaymentMethodById,
        PaymentMethod,
        PaymentMethodDTO,
        QueryGetPaymentMethodById,
        IRepositoryPaymentMethod,
        TestFactoryPaymentMethod
    >
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryPaymentMethod>();
        return base.InitializeAsync();
    }

    protected override Application.Expenses.Queries.GetById.TestHandlerGetPaymentMethodById CreateHandler() =>
        new(GetMock<IRepositoryPaymentMethod>().Object);
    
    protected override QueryGetPaymentMethodById GetQuery() => new() { Id = 1 };

    [Fact]
    public async Task GetOne_ValidData_ReturnedOne()
    {
        await GetOne_ValidData_ReturnedOne_Base((inputEntity, resultDTO) =>
        {
            Assert.Equal(inputEntity.Id, resultDTO.Id);
            Assert.Equal(inputEntity.Name, resultDTO.Name);
            Assert.Equal(inputEntity.Limit, resultDTO.Limit);
        });
    }

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnedNull() =>
        await GetOne_NoEntryAtPassedID_ReturnedNull_Base();
}