using System.Threading.Tasks;
using Application.DTOs;
using Application.Expenses.Queries.GetAll;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllCategories))]
public class TestHandlerGetAllCategories
    : TestHandlerGetAll<
        HandlerGetAllCategories,
        CategoryExpenses,
        CategoryDTO,
        QueryGetAllCategories,
        IRepositoryCategoryExpenses,
        TestFactoryCategoryExpenses
    >
{
    protected override HandlerGetAllCategories CreateHandler() =>
        new(GetMock<IRepositoryCategoryExpenses>().Object);
    
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