using System.Threading.Tasks;
using Application.DTOs.OutputDTOs;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Expenses.Queries.GetById;
using Application.Tests.Factories;
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
        TestFactoryCategoryExpenses
    >
{
    protected override HandlerGetCategoryById CreateHandler() =>
        new(GetMock<IRepositoryExpenseCategory>().Object);
    
    protected override QueryGetCategoryById GetQuery() => new() { Id = 1 };

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