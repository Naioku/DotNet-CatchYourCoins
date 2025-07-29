using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries.GetAll;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllPaymentMethods))]
public class TestHandlerGetAllPaymentMethods
    : TestHandlerGetAll<
        HandlerGetAllPaymentMethods,
        ExpensePaymentMethod,
        PaymentMethodDTO,
        QueryGetAllPaymentMethods,
        IRepositoryExpensePaymentMethod,
        TestFactoryPaymentMethod
    >
{
    protected override HandlerGetAllPaymentMethods CreateHandler() =>
        new(GetMock<IRepositoryExpensePaymentMethod>().Object);

    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base((inputEntity, resultDTO) =>
        {
            Assert.Equal(inputEntity.Name, resultDTO.Name);
            Assert.Equal(inputEntity.Limit, resultDTO.Limit);
        });

    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();
}