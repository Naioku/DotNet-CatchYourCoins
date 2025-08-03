using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Expenses.Commands.Create;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Create.CreateCategory;

[TestSubject(typeof(HandlerCreateCategory))]
public class TestHandlerCreateCategory
    : TestHandlerCreate<
        HandlerCreateCategory,
        ExpenseCategory,
        InputDTOExpenseCategory,
        CommandCreateCategory,
        IRepositoryExpenseCategory
    >
{
    protected override HandlerCreateCategory CreateHandler()
    {
        return new HandlerCreateCategory(
            GetMock<IRepositoryExpenseCategory>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override CommandCreateCategory GetCommand(InputDTOExpenseCategory dto) => new() { Data = dto };

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