using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Expenses.Commands.Create;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Commands.Create.CreateExpense;

[TestSubject(typeof(HandlerCreateExpense))]
public class TestHandlerCreateExpense
    : TestHandlerCreate<
        HandlerCreateExpense,
        Expense,
        InputDTOExpense,
        CommandCreateExpense,
        IRepositoryExpense
    >
{
    protected override HandlerCreateExpense CreateHandler()
    {
        return new HandlerCreateExpense(
            GetMock<IRepositoryExpense>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override CommandCreateExpense GetCommand(InputDTOExpense dto) => new() { Data = dto };

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
    
    [Fact]
    public async Task Create_RepositoryThrowsException_EntityNotCreated() =>
        await Create_RepositoryThrowsException_EntityNotCreated_Base();
    
    [Fact]
    public async Task Create_UnitOfWorkThrowsException_EntityNotCreated() =>
        await Create_UnitOfWorkThrowsException_EntityNotCreated_Base();
}