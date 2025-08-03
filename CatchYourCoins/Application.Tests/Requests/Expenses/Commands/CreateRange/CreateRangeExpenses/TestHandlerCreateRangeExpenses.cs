using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Expenses.Commands.CreateRange;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Commands.CreateRange.CreateRangeExpenses;

[TestSubject(typeof(HandlerCreateRangeExpenses))]
public class TestHandlerCreateRangeExpense
    : TestHandlerCreateRange<
        HandlerCreateRangeExpenses,
        Expense,
        InputDTOExpense,
        CommandCreateRangeExpenses,
        IRepositoryExpense
    >
{
    protected override HandlerCreateRangeExpenses CreateHandler()
    {
        return new HandlerCreateRangeExpenses(
            GetMock<IRepositoryExpense>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override CommandCreateRangeExpenses GetCommand(List<InputDTOExpense> dtos) => new() { Data = dtos };

    [Fact]
    public async Task Create_ValidData_EntitiesCreated() =>
        await Create_ValidData_EntitiesCreated_Base();
    
    [Fact]
    public async Task Create_RepositoryThrowsException_EntityNotCreated() =>
        await Create_RepositoryThrowsException_EntitiesNotCreated_Base();
    
    [Fact]
    public async Task Create_UnitOfWorkThrowsException_EntitiesNotCreated() =>
        await Create_UnitOfWorkThrowsException_EntitiesNotCreated_Base();
}