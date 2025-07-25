using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Queries;

[TestSubject(typeof(HandlerGetAllExpenses))]
public class HandlerGetAllExpensesTest : CQRSHandlerTestBase<HandlerGetAllExpenses>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryExpense>();
        return base.InitializeAsync();
    }

    protected override HandlerGetAllExpenses CreateHandler() =>
        new(GetMock<IRepositoryExpense>().Object);

    
    [Fact]
    public async Task GetAll_ValidData_ReturnsAll()
    {
        // Arrange
        List<Expense> expenses = FactoryExpense.CreateEntities(TestFactoryUsers.DefaultUser1Authenticated, 5);
        GetMock<IRepositoryExpense>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync(expenses);
        
        QueryGetAllExpenses query = new();

        // Act
        Result<IReadOnlyList<ExpenseDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        // ReSharper disable once InconsistentNaming
        IReadOnlyList<ExpenseDTO> expenseDTOs = result.Value;

        for (var i = 0; i < expenseDTOs.Count; i++)
        {
            ExpenseDTO dto = expenseDTOs[i];
            Assert.Equal(expenses[i].Id, dto.Id);
            Assert.Equal(expenses[i].Amount, dto.Amount);
            Assert.Equal(expenses[i].Date, dto.Date);
            Assert.Equal(expenses[i].Description, dto.Description);
            Assert.NotNull(expenses[i].Category);
            Assert.NotNull(expenses[i].PaymentMethod);
            Assert.Equal(expenses[i].Category.Name, dto.Category);
            Assert.Equal(expenses[i].PaymentMethod.Name, dto.PaymentMethod);
        }
    }
    
    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnsNull()
    {
        // Arrange
        GetMock<IRepositoryExpense>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync([]);
        
        QueryGetAllExpenses query = new();

        // Act
        Result<IReadOnlyList<ExpenseDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Value);
    }
}