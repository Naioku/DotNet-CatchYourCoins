using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries.GetAll;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetAll;

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
        List<Expense> entities = FactoryExpense.CreateEntities(TestFactoryUsers.DefaultUser1Authenticated, 5);
        GetMock<IRepositoryExpense>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync(entities);
        
        QueryGetAllExpenses query = new();

        // Act
        Result<IReadOnlyList<ExpenseDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        // ReSharper disable once InconsistentNaming
        IReadOnlyList<ExpenseDTO> dtos = result.Value;

        for (var i = 0; i < dtos.Count; i++)
        {
            ExpenseDTO dto = dtos[i];
            Assert.Equal(entities[i].Id, dto.Id);
            Assert.Equal(entities[i].Amount, dto.Amount);
            Assert.Equal(entities[i].Date, dto.Date);
            Assert.Equal(entities[i].Description, dto.Description);
            Assert.NotNull(entities[i].Category);
            Assert.NotNull(entities[i].PaymentMethod);
            Assert.Equal(entities[i].Category.Name, dto.Category);
            Assert.Equal(entities[i].PaymentMethod.Name, dto.PaymentMethod);
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