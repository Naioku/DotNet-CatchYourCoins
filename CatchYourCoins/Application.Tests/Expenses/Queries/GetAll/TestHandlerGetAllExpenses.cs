using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Expenses.Queries.GetAll;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllExpenses))]
public class TestHandlerGetAllExpenses
    : TestHandlerGetAll<
        HandlerGetAllExpenses,
        Domain.Dashboard.Entities.Expenses.Expense,
        OutputDTOExpense,
        QueryGetAllExpenses,
        IRepositoryExpense,
        TestFactoryExpense
    >
{
    protected override HandlerGetAllExpenses CreateHandler() =>
        new(GetMock<IRepositoryExpense>().Object);
    
    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base((inputEntity, resultDTO) =>
        {
            Assert.Equal(inputEntity.Id, resultDTO.Id);
            Assert.Equal(inputEntity.Amount, resultDTO.Amount);
            Assert.Equal(inputEntity.Date, resultDTO.Date);
            Assert.Equal(inputEntity.Description, resultDTO.Description);
            Assert.NotNull(inputEntity.Category);
            Assert.NotNull(inputEntity.PaymentMethod);
            Assert.Equal(inputEntity.Category.Name, resultDTO.Category);
            Assert.Equal(inputEntity.PaymentMethod.Name, resultDTO.PaymentMethod);
        });
    
    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();
}