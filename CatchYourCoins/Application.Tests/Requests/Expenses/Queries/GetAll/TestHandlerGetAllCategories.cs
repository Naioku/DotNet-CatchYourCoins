using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Expenses.Queries.GetAll;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllCategories))]
public class TestHandlerGetAllCategories
    : TestHandlerGetAll<
        HandlerGetAllCategories,
        ExpenseCategory,
        OutputDTOExpenseCategory,
        QueryGetAllCategories,
        IRepositoryExpenseCategory
    >
{
    protected override HandlerGetAllCategories CreateHandler() =>
        new(
            GetMock<IRepositoryExpenseCategory>().Object,
            GetMock<IMapper>().Object
        );

    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base();

    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();
}