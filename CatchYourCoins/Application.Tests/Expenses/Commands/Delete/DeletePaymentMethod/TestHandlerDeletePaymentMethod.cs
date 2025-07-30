using System.Threading.Tasks;
using Application.Expenses.Commands.Delete;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeletePaymentMethod;

[TestSubject(typeof(HandlerDeletePaymentMethod))]
public class TestHandlerDeletePaymentMethod
    : TestHandlerDelete<
        HandlerDeletePaymentMethod,
        ExpensePaymentMethod,
        CommandDeletePaymentMethod,
        IRepositoryExpensePaymentMethod,
        TestFactoryExpensePaymentMethod,
        IUnitOfWork
    >
{
    protected override HandlerDeletePaymentMethod CreateHandler()
    {
        return new HandlerDeletePaymentMethod(
            GetMock<IRepositoryExpensePaymentMethod>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }
    
    protected override CommandDeletePaymentMethod GetCommand() => new() { Id = 1 };

    [Fact]
    public async Task DeleteOne_ValidData_DeletedEntity() =>
        await DeleteOne_ValidData_DeletedEntity_Base();

    [Fact]
    public async Task DeleteOne_NoEntryAtPassedID_DeletedNothing() =>
        await DeleteOne_NoEntryAtPassedID_DeletedNothing_Base();
}