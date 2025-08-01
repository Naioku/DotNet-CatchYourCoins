using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Expenses.Commands.Create;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Create.CreatePaymentMethod;

[TestSubject(typeof(HandlerCreatePaymentMethod))]
public class TestHandlerCreatePaymentMethod
    : TestHandlerCreate<
        HandlerCreatePaymentMethod,
        ExpensePaymentMethod,
        InputDTOExpensePaymentMethod,
        CommandCreatePaymentMethod,
        IRepositoryExpensePaymentMethod,
        TestFactoryExpensePaymentMethod
    >
{
    private readonly InputDTOExpensePaymentMethod _dto = new()
    {
        Name = "Test",
        Limit = 1000
    };

    protected override HandlerCreatePaymentMethod CreateHandler()
    {
        return new HandlerCreatePaymentMethod(
            GetMock<IRepositoryExpensePaymentMethod>().Object,
            GetMock<IServiceCurrentUser>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override InputDTOExpensePaymentMethod GetInputDTO() => _dto;

    protected override CommandCreatePaymentMethod GetCommand() => new() { Data = _dto };

    protected override ExpensePaymentMethod GetMappedEntity() => new()
    {
        Name = _dto.Name,
        Limit = _dto.Limit,
        UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
    };

    [Fact]
    public async Task Create_ValidData_EntryCreated() =>
        await Create_ValidData_EntityCreated_Base();
}