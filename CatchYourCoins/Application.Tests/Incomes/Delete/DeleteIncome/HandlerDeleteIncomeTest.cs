using System.Threading.Tasks;
using Application.Incomes.Delete;
using Application.Tests.Factories;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Delete.DeleteIncome;

[TestSubject(typeof(HandlerDeleteIncome))]
public class HandlerDeleteIncomeTest
    : TestHandlerDelete<
        HandlerDeleteIncome,
        Income,
        CommandDeleteIncome,
        IRepositoryIncome,
        TestFactoryIncome,
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