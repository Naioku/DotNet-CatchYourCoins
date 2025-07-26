using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries.GetAll;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetAll;

[TestSubject(typeof(Application.Expenses.Queries.GetAll.TestHandlerGetAllPaymentMethods))]
public class TestHandlerGetAllPaymentMethods
    : TestHandlerGetAll<Application.Expenses.Queries.GetAll.TestHandlerGetAllPaymentMethods, PaymentMethod, PaymentMethodDTO, QueryGetAllPaymentMethods, IRepositoryPaymentMethod, TestFactoryPaymentMethod>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryPaymentMethod>();
        return base.InitializeAsync();
    }

    protected override Application.Expenses.Queries.GetAll.TestHandlerGetAllPaymentMethods CreateHandler() =>
        new(GetMock<IRepositoryPaymentMethod>().Object);

    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base((inputEntity, dtoResult) =>
        {
            Assert.Equal(inputEntity.Name, dtoResult.Name);
            Assert.Equal(inputEntity.Limit, dtoResult.Limit);
        });

    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();
}