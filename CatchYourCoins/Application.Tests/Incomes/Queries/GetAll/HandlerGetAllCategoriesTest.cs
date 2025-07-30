using System.Threading.Tasks;
using Application.DTOs.OutputDTOs;
using Application.DTOs.OutputDTOs.Incomes;
using Application.Incomes.Queries.GetAll;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllCategories))]
public class HandlerGetAllCategoriesTest
    : TestHandlerGetAll<
        HandlerGetAllCategories,
        IncomeCategory,
        OutputDTOIncomeCategory,
        QueryGetAllCategories,
        IRepositoryIncomeCategory,
        TestFactoryIncomeCategory
    >
{
    protected override HandlerGetAllCategories CreateHandler() =>
        new(GetMock<IRepositoryIncomeCategory>().Object);
    
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