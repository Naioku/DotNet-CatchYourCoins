using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Incomes;
using Application.Incomes.Queries.GetAll;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllIncomes))]
public class HandlerGetAllIncomesTest
    : TestHandlerGetAll<
        HandlerGetAllIncomes,
        Domain.Dashboard.Entities.Incomes.Income,
        OutputDTOIncome,
        QueryGetAllIncomes,
        IRepositoryIncome,
        TestFactoryIncome
    >
{
    protected override HandlerGetAllIncomes CreateHandler() =>
        new(GetMock<IRepositoryIncome>().Object);
    
    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base((inputEntity, resultDTO) =>
        {
            Assert.Equal(inputEntity.Id, resultDTO.Id);
            Assert.Equal(inputEntity.Amount, resultDTO.Amount);
            Assert.Equal(inputEntity.Date, resultDTO.Date);
            Assert.Equal(inputEntity.Description, resultDTO.Description);
            Assert.NotNull(inputEntity.Category);
            Assert.Equal(inputEntity.Category.Name, resultDTO.Category);
        });
    
    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();
}