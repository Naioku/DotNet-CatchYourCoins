using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Expenses.Queries.GetById;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Queries.GetById;

[TestSubject(typeof(HandlerGetCategoryById))]
public class TestHandlerGetCategoryById
    : TestHandlerGetById<
        HandlerGetCategoryById,
        ExpenseCategory,
        OutputDTOExpenseCategory,
        QueryGetCategoryById,
        IRepositoryExpenseCategory
    >
{
    protected override HandlerGetCategoryById CreateHandler() =>
        new(
            GetMock<IRepositoryExpenseCategory>().Object,
            GetMock<IMapper>().Object
        );

    protected override QueryGetCategoryById GetQuery() => new() { Id = 1 };

    [Fact]
    public async Task GetOne_ValidData_ReturnedOne() =>
        await GetOne_ValidData_ReturnedOne_Base();

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnedNull() =>
        await GetOne_NoEntryAtPassedID_ReturnedNull_Base();
}