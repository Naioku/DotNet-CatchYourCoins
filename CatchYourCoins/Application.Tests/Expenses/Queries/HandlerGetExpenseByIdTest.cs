using System;
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
public class HandlerGetExpenseByIdTest
{
    [Fact]
    public async Task GetExpense_ValidDataAndExpenseExists_ReturnExpense()
    {
        // Arrange
        var query = new QueryGetExpenseById { Id = 1 };

        var mockRepository = new Mock<IRepositoryExpense>();

        var expense = new Expense
        {
            Id = 1,
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            UserId = TestFactoryUsers.DefaultUser1().Id,
            CategoryId = 1,
            Category = new Category
            {
                Name = "Test",
                UserId = TestFactoryUsers.DefaultUser1().Id,
            },
            PaymentMethodId = 1,
            PaymentMethod = new PaymentMethod
            {
                Name = "Test",
                UserId = TestFactoryUsers.DefaultUser1().Id,
            },
        };

        mockRepository
            .Setup(m => m.GetExpenseByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(expense);

        var handler = new HandlerGetExpenseById(mockRepository.Object);

        // Act
        Result<ExpenseDTO> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        ExpenseDTO expenseDTO = result.Value;
        Assert.NotNull(expenseDTO);
        Assert.Equal(expenseDTO.Id, query.Id);
        Assert.Equal(expenseDTO.Amount, expense.Amount);
        Assert.Equal(expenseDTO.Date, expense.Date);
        Assert.Equal(expenseDTO.Description, expense.Description);
        Assert.NotNull(expense.Category);
        Assert.NotNull(expense.PaymentMethod);
        Assert.Equal(expenseDTO.Category, expense.Category.Name);
        Assert.Equal(expenseDTO.PaymentMethod, expense.PaymentMethod.Name);
    }

    [Fact]
    public async Task GetExpense_InvalidUserAndExpenseNotExists_ReturnNull()
    {
        // Arrange
        var query = new QueryGetExpenseById { Id = 1 };

        var mockRepository = new Mock<IRepositoryExpense>();
        mockRepository
            .Setup(m => m.GetExpenseByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Expense)null);

        var handler = new HandlerGetExpenseById(mockRepository.Object);

        // Act
        Result<ExpenseDTO> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);

        ExpenseDTO expenseDTO = result.Value;
        Assert.Null(expenseDTO);
    }
}