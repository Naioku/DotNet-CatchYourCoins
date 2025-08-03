using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Expenses.Queries.GetById;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetById;

[TestSubject(typeof(HandlerGetExpenseById))]
public class TestHandlerGetExpenseById
    : TestHandlerGetById<
        HandlerGetExpenseById,
        Expense,
        OutputDTOExpense,
        QueryGetExpenseById,
        IRepositoryExpense
    >
{
    protected override HandlerGetExpenseById CreateHandler() =>
        new(
            GetMock<IRepositoryExpense>().Object,
            GetMock<IMapper>().Object
        );

    protected override QueryGetExpenseById GetQuery() => new() { Id = 1 };

    [Fact]
    public async Task GetOne_ValidData_ReturnedOne() =>
        await GetOne_ValidData_ReturnedOne_Base();

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnedNull() =>
        await GetOne_NoEntryAtPassedID_ReturnedNull_Base();
}