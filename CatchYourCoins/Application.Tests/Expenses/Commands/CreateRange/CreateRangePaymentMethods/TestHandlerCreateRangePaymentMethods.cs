using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Expenses;
using Application.Expenses.Commands.CreateRange;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.CreateRange.CreateRangePaymentMethods;

[TestSubject(typeof(HandlerCreateRangePaymentMethods))]
public class TestHandlerCreateRangePaymentMethods
    : TestHandlerCreateRange<
        HandlerCreateRangePaymentMethods,
        ExpensePaymentMethod,
        InputDTOExpensePaymentMethod,
        CommandCreateRangePaymentMethods,
        IRepositoryExpensePaymentMethod,
        TestFactoryExpensePaymentMethod
    >
{
    private readonly IList<InputDTOExpensePaymentMethod> _inputDTOs = [
        new()
        {
            Name = "Test1",
            Limit = 1000
        },
        new()
        {
            Name = "Test2",
            Limit = 2000
        },
    ];

    protected override HandlerCreateRangePaymentMethods CreateHandler()
    {
        return new HandlerCreateRangePaymentMethods(
            GetMock<IRepositoryExpensePaymentMethod>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override IEnumerable<InputDTOExpensePaymentMethod> GetInputDTOs() => _inputDTOs;
    protected override CommandCreateRangePaymentMethods GetCommand() => new() { Data = _inputDTOs };

    protected override IList<ExpensePaymentMethod> GetMappedEntities() =>
        _inputDTOs.Select(dto => new ExpensePaymentMethod
        {
            Name = dto.Name,
            Limit = dto.Limit,
            UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
        }).ToList();

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}