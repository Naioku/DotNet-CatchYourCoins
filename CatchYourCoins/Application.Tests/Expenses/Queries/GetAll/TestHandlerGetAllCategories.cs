using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Expenses.Queries.GetAll;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllCategories))]
public class TestHandlerGetAllCategories
    : TestHandlerGetAll<
        HandlerGetAllCategories,
        ExpenseCategory,
        OutputDTOExpenseCategory,
        QueryGetAllCategories,
        IRepositoryExpenseCategory,
        TestFactoryExpenseCategory
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

    protected override IReadOnlyList<OutputDTOExpenseCategory> GetMappedDTOs(List<ExpenseCategory> entity)
    {
        return entity.Select(e => new OutputDTOExpenseCategory
        {
            Id = e.Id,
            Name = e.Name,
            Limit = e.Limit,
        }).ToList();
    }
}