using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Expenses;
using Application.Expenses.Queries.GetById;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetById;

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
    public async Task GetOne_ValidData_ReturnOne()
    {
        // Arrange
        Expense entity = FactoryExpense.CreateEntity(TestFactoryUsers.DefaultUser1Authenticated);
        GetMock<IRepositoryExpense>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == entity.Id
            )))
            .ReturnsAsync(entity);
        
        QueryGetExpenseById query = new() { Id = entity.Id };


        // Act
        Result<ExpenseDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        ExpenseDTO dto = result.Value;
        Assert.Equal(query.Id, dto.Id);
        Assert.Equal(entity.Amount, dto.Amount);
        Assert.Equal(entity.Date, dto.Date);
        Assert.Equal(entity.Description, dto.Description);
        Assert.NotNull(entity.Category);
        Assert.NotNull(entity.PaymentMethod);
        Assert.Equal(entity.Category.Name, dto.Category);
        Assert.Equal(entity.PaymentMethod.Name, dto.PaymentMethod);
    }

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnNull()
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
        Assert.Null(result.Value);
    }
}