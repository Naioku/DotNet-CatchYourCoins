using System.Threading.Tasks;
using Application.DTOs;
using Application.Incomes.Queries.GetAll;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllCategories))]
public class HandlerGetAllCategoriesTest
    : TestHandlerGetAll<
        HandlerGetAllCategories,
        CategoryIncomes,
        CategoryDTO,
        QueryGetAllCategories,
        IRepositoryCategoryIncomes,
        TestFactoryCategoryIncomes
    >
{
    protected override HandlerGetAllCategories CreateHandler() =>
        new(GetMock<IRepositoryCategoryIncomes>().Object);
    
    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base((inputEntity, dtoResult) =>
        {
            Assert.Equal(inputEntity.Name, dtoResult.Name);
            Assert.Equal(inputEntity.Limit, dtoResult.Limit);
        });
    
    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();
}