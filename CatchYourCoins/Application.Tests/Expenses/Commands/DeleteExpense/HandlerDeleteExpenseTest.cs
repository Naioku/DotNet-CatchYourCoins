using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.DeleteExpense;

[TestSubject(typeof(HandlerDeleteExpense))]
public class HandlerDeleteExpenseTest : CQRSHandlerTestBase<HandlerDeleteExpense>
{
    public override Task InitializeAsync()
    {
        RegisterMock<IRepositoryExpense>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected override HandlerDeleteExpense CreateHandler()
    {
        return new HandlerDeleteExpense(
            GetMock<IRepositoryExpense>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }
    
    [Fact]
    public async Task Delete_ValidData_EntryDeleted()
    {
        // Arrange
        Expense expense = FactoryExpense.CreateEntity(TestFactoryUsers.DefaultUser1Authenticated);
        GetMock<IRepositoryExpense>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == expense.Id
            )))
            .ReturnsAsync(expense);
    
        CommandDeleteExpense command = new() { Id = expense.Id };
    
        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);
    
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        GetMock<IRepositoryExpense>().Verify(m => m.Delete(It.Is<Expense>(e => e.Id == command.Id)));
        GetMock<IUnitOfWork>().Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Delete_NoEntryInDBAtPassedID_EntryNotDeleted()
    {
        // Arrange
        GetMock<IRepositoryExpense>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == 1
            )))
            .ReturnsAsync((Expense)null);
    
        CommandDeleteExpense command = new() { Id = 1 };
    
        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);
    
        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        GetMock<IRepositoryExpense>().Verify(m => m.Delete(It.IsAny<Expense>()), Times.Never);
    }
}