using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Incomes;
using Application.Incomes.Queries.GetById;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Queries.GetById;

[TestSubject(typeof(HandlerGetIncomeById))]
public class HandlerGetIncomeByIdTest
    : TestHandlerGetById<
        HandlerGetIncomeById,
        Income,
        OutputDTOIncome,
        QueryGetIncomeById,
        IRepositoryIncome,
        TestFactoryIncome
    >
{
    protected override HandlerGetIncomeById CreateHandler() =>
        new(GetMock<IRepositoryIncome>().Object);
    
    protected override QueryGetIncomeById GetQuery() => new() { Id = 1 };

    [Fact]
    public async Task GetOne_ValidData_ReturnedOne()
    {
        await GetOne_ValidData_ReturnedOne_Base((inputEntity, resultDTO) =>
        {
            Assert.Equal(inputEntity.Id, resultDTO.Id);
            Assert.Equal(inputEntity.Amount, resultDTO.Amount);
            Assert.Equal(inputEntity.Date, resultDTO.Date);
            Assert.Equal(inputEntity.Description, resultDTO.Description);
            Assert.NotNull(inputEntity.Category);
            Assert.Equal(inputEntity.Category.Name, resultDTO.Category);
        });
    }

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnedNull() =>
        await GetOne_NoEntryAtPassedID_ReturnedNull_Base();
}