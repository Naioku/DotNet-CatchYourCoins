using System.Threading.Tasks;
using Application.Requests.Expenses.Commands.Delete;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Commands.Delete.DeleteExpense;

[TestSubject(typeof(HandlerDeleteExpense))]
public class TestHandlerDeleteExpense
    : TestHandlerDelete<
        HandlerDeleteExpense,
        Expense,
        CommandDeleteExpense,
        IRepositoryExpense,
        IUnitOfWork
    >
{
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