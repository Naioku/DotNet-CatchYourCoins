using System.Threading.Tasks;
using Application.Expenses.Commands.Delete;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeleteExpense;

[TestSubject(typeof(HandlerDeleteExpense))]
public class TestHandlerDeleteExpense
    : TestHandlerDelete<HandlerDeleteExpense, Expense, CommandDeleteExpense, IRepositoryExpense, TestFactoryExpense, IUnitOfWork>
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
    
    protected override CommandDeleteExpense GetCommand() => new() { Id = 1 };
    
    [Fact]
    public async Task DeleteOne_ValidData_DeletedEntity() =>
        await DeleteOne_ValidData_DeletedEntity_Base();

    [Fact]
    public async Task DeleteOne_NoEntryAtPassedID_DeletedNothing() =>
        await DeleteOne_NoEntryAtPassedID_DeletedNothing_Base();
}