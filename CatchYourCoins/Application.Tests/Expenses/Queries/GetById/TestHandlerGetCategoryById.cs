using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Expenses.Queries.GetById;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetById;

[TestSubject(typeof(HandlerGetCategoryById))]
public class TestHandlerGetCategoryById
    : TestHandlerGetById<
        HandlerGetCategoryById,
        ExpenseCategory,
        OutputDTOExpenseCategory,
        QueryGetCategoryById,
        IRepositoryExpenseCategory,
        TestFactoryExpenseCategory
    >
{
    protected override HandlerGetCategoryById CreateHandler() =>
        new(
            GetMock<IRepositoryExpenseCategory>().Object,
            GetMock<IMapper>().Object
        );

    protected override QueryGetCategoryById GetQuery() => new() { Id = 1 };
    protected override OutputDTOExpenseCategory GetMappedDTO(ExpenseCategory entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };

    [Fact]
    public async Task GetOne_ValidData_ReturnedOne()
    {
        await GetOne_ValidData_ReturnedOne_Base();
    }

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnedNull() =>
        await GetOne_NoEntryAtPassedID_ReturnedNull_Base();
}