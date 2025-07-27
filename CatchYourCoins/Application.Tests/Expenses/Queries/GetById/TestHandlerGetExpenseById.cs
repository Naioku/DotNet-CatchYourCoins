using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries.GetById;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetById;

[TestSubject(typeof(HandlerGetExpenseById))]
public class TestHandlerGetExpenseById
    : TestHandlerGetById<
        HandlerGetExpenseById,
        Expense,
        ExpenseDTO,
        QueryGetExpenseById,
        IRepositoryExpense,
        TestFactoryExpense
    >
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryExpense>();
        return base.InitializeAsync();
    }

    protected override HandlerGetExpenseById CreateHandler() =>
        new(GetMock<IRepositoryExpense>().Object);
    
    protected override QueryGetExpenseById GetQuery() => new() { Id = 1 };

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
            Assert.NotNull(inputEntity.PaymentMethod);
            Assert.Equal(inputEntity.Category.Name, resultDTO.Category);
            Assert.Equal(inputEntity.PaymentMethod.Name, resultDTO.PaymentMethod);
        });
    }

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnedNull() =>
        await GetOne_NoEntryAtPassedID_ReturnedNull_Base();
}