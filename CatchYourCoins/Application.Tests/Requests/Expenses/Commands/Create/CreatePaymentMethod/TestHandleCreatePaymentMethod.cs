using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Expenses.Commands.Create;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Commands.Create.CreatePaymentMethod;

[TestSubject(typeof(HandlerCreatePaymentMethod))]
public class TestHandlerCreatePaymentMethod
    : TestHandlerCreate<
        HandlerCreatePaymentMethod,
        ExpensePaymentMethod,
        InputDTOExpensePaymentMethod,
        CommandCreatePaymentMethod,
        IRepositoryExpensePaymentMethod
    >
{
    protected override HandlerCreatePaymentMethod CreateHandler()
    {
        return new HandlerCreatePaymentMethod(
            GetMock<IRepositoryExpensePaymentMethod>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override CommandCreatePaymentMethod GetCommand(InputDTOExpensePaymentMethod dto) => new() { Data = dto };

    [Fact]
    public async Task Create_ValidData_EntryCreated() =>
        await Create_ValidData_EntityCreated_Base();
    
    [Fact]
    public async Task Create_RepositoryThrowsException_EntityNotCreated() =>
        await Create_RepositoryThrowsException_EntityNotCreated_Base();
    
    [Fact]
    public async Task Create_UnitOfWorkThrowsException_EntityNotCreated() =>
        await Create_UnitOfWorkThrowsException_EntityNotCreated_Base();
}