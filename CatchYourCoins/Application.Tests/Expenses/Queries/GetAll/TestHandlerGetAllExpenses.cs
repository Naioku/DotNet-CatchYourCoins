using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries.GetAll;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetAll;

[TestSubject(typeof(Application.Expenses.Queries.GetAll.TestHandlerGetAllExpenses))]
public class TestHandlerGetAllExpenses
    : TestHandlerGetAll<Application.Expenses.Queries.GetAll.TestHandlerGetAllExpenses, Expense, ExpenseDTO, QueryGetAllExpenses, IRepositoryExpense, TestFactoryExpense>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryExpense>();
        return base.InitializeAsync();
    }

    protected override Application.Expenses.Queries.GetAll.TestHandlerGetAllExpenses CreateHandler() =>
        new(GetMock<IRepositoryExpense>().Object);
    
    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base((inputEntity, dtoResult) =>
        {
            Assert.Equal(inputEntity.Id, dtoResult.Id);
            Assert.Equal(inputEntity.Amount, dtoResult.Amount);
            Assert.Equal(inputEntity.Date, dtoResult.Date);
            Assert.Equal(inputEntity.Description, dtoResult.Description);
            Assert.NotNull(inputEntity.Category);
            Assert.NotNull(inputEntity.PaymentMethod);
            Assert.Equal(inputEntity.Category.Name, dtoResult.Category);
            Assert.Equal(inputEntity.PaymentMethod.Name, dtoResult.PaymentMethod);
        });
    
    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();
}