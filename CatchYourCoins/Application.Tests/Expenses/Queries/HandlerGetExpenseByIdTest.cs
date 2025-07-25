using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries;
using Application.Tests.Factories;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Queries;

[TestSubject(typeof(HandlerGetExpenseById))]
public class HandlerGetExpenseByIdTest : CQRSHandlerTestBase<HandlerGetExpenseById>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryExpense>();
        return base.InitializeAsync();
    }

    protected override HandlerGetExpenseById CreateHandler() =>
        new(GetMock<IRepositoryExpense>().Object);

    [Fact]
    public async Task GetExpense_ValidDataAndExpenseExists_ReturnExpense()
    {
        // Arrange
        Expense expense = TestFactoryExpense.CreateExpense(TestFactoryUsers.DefaultUser1Authenticated);
        GetMock<IRepositoryExpense>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == expense.Id
            )))
            .ReturnsAsync(expense);
        
        QueryGetExpenseById query = new() { Id = expense.Id };


        // Act
        Result<ExpenseDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        ExpenseDTO expenseDTO = result.Value;
        Assert.NotNull(expenseDTO);
        Assert.Equal(query.Id, expenseDTO.Id);
        Assert.Equal(expense.Amount, expenseDTO.Amount);
        Assert.Equal(expense.Date, expenseDTO.Date);
        Assert.Equal(expense.Description, expenseDTO.Description);
        Assert.NotNull(expense.Category);
        Assert.NotNull(expense.PaymentMethod);
        Assert.Equal(expense.Category.Name, expenseDTO.Category);
        Assert.Equal(expense.PaymentMethod.Name, expenseDTO.PaymentMethod);
    }

    [Fact]
    public async Task GetExpense_NoExpense_ReturnNull()
    {
        // Arrange
        QueryGetExpenseById query = new() { Id = 1 };

        GetMock<IRepositoryExpense>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == query.Id
            )))
            .ReturnsAsync((Expense)null);

        // Act
        Result<ExpenseDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);

        ExpenseDTO expenseDTO = result.Value;
        Assert.Null(expenseDTO);
    }
}