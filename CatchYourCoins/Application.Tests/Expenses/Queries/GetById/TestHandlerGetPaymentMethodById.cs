using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Expenses.Queries.GetById;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetById;

[TestSubject(typeof(HandlerGetPaymentMethodById))]
public class TestHandlerGetPaymentMethodById
    : TestHandlerGetById<
        HandlerGetPaymentMethodById,
        ExpensePaymentMethod,
        OutputDTOExpensePaymentMethod,
        QueryGetPaymentMethodById,
        IRepositoryExpensePaymentMethod
    >
{
    protected override HandlerGetPaymentMethodById CreateHandler() =>
        new(
            GetMock<IRepositoryExpensePaymentMethod>().Object,
            GetMock<IMapper>().Object
        );

    protected override QueryGetPaymentMethodById GetQuery() => new() { Id = 1 };

    [Fact]
    public async Task GetOne_ValidData_ReturnedOne() =>
        await GetOne_ValidData_ReturnedOne_Base();

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnedNull() =>
        await GetOne_NoEntryAtPassedID_ReturnedNull_Base();
}