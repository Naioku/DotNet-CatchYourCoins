using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Expenses.Queries.GetAll;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllPaymentMethods))]
public class TestHandlerGetAllPaymentMethods
    : TestHandlerGetAll<
        HandlerGetAllPaymentMethods,
        ExpensePaymentMethod,
        OutputDTOExpensePaymentMethod,
        QueryGetAllPaymentMethods,
        IRepositoryExpensePaymentMethod
    >
{
    protected override HandlerGetAllPaymentMethods CreateHandler() =>
        new(
            GetMock<IRepositoryExpensePaymentMethod>().Object,
            GetMock<IMapper>().Object
        );

    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base();

    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();
}