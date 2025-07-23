using System.Threading;
using System.Threading.Tasks;
using Application.Expenses.Commands;
using Application.Tests.Factories;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Commands.DeleteExpense;

[TestSubject(typeof(HandlerDeleteExpense))]
public class HandlerDeleteExpenseTest : IAsyncLifetime
{
    private Mock<IRepositoryExpense> _mockRepository;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private HandlerDeleteExpense _handler;
    
    public Task InitializeAsync()
    {
        _mockRepository = new Mock<IRepositoryExpense>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        
        _handler = new HandlerDeleteExpense(
            _mockRepository.Object,
            _mockUnitOfWork.Object
        );
        
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _mockRepository = null;
        _mockUnitOfWork = null;
        _handler = null;
        return Task.CompletedTask;
    }

    [Fact]
    public async Task DeleteExpense_ValidData_DeleteExpense()
    {
        // Arrange
        Expense expense = TestFactoryExpense.CreateExpense(TestFactoryUsers.DefaultUser1Authenticated);
        _mockRepository
            .Setup(m => m.GetExpenseByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(expense);
        
        CommandDeleteExpense command = new() { Id = expense.Id };

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        _mockRepository.Verify(m => m.DeleteExpense(It.Is<Expense>(e => e.Id == command.Id)));
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteExpense_NoExpenseUnderPassedID_NotDeleteExpense()
    {
        // Arrange
        _mockRepository
            .Setup(m => m.GetExpenseByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Expense)null);
        
        CommandDeleteExpense command = new() { Id = 1 };
        
        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        _mockRepository.Verify(m => m.DeleteExpense(It.IsAny<Expense>()), Times.Never);
    }
}