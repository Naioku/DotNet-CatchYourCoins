using System.Threading.Tasks;
using Application.DTOs.OutputDTOs;
using Application.DTOs.OutputDTOs.Incomes;
using Application.Incomes.Queries.GetById;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Queries.GetById;

[TestSubject(typeof(HandlerGetCategoryById))]
public class HandlerGetCategoryByIdTest
    : TestHandlerGetById<
        HandlerGetCategoryById,
        IncomeCategory,
        OutputDTOIncomeCategory,
        QueryGetCategoryById,
        IRepositoryIncomeCategory,
        TestFactoryIncomeCategory
    >
{
    protected override HandlerGetCategoryById CreateHandler() =>
        new(GetMock<IRepositoryIncomeCategory>().Object);
    
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