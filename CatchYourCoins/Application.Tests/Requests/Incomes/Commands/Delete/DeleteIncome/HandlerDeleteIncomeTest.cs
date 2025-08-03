using System.Threading.Tasks;
using Application.Requests.Incomes.Commands.Delete;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Incomes.Commands.Delete.DeleteIncome;

[TestSubject(typeof(HandlerDeleteIncome))]
public class HandlerDeleteIncomeTest
    : TestHandlerDelete<
        HandlerDeleteIncome,
        Income,
        CommandDeleteIncome,
        IRepositoryIncome,
        IUnitOfWork
    >
{
    protected override HandlerDeleteIncome CreateHandler()
    {
        return new HandlerDeleteIncome(
            GetMock<IRepositoryIncome>().Object,
            GetMock<IUnitOfWork>().Object
        );
    }
    
    protected override CommandDeleteIncome GetCommand() => new() { Id = 1 };
    
    [Fact]
    public async Task DeleteOne_ValidData_DeletedEntity() =>
        await DeleteOne_ValidData_DeletedEntity_Base();

    [Fact]
    public async Task DeleteOne_NoEntryAtPassedID_DeletedNothing() =>
        await DeleteOne_NoEntryAtPassedID_DeletedNothing_Base();
}