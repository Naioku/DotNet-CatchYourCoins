using System;
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

namespace Application.Tests.Expenses.Commands.CreateRange.CreateRangeExpenses;

[TestSubject(typeof(HandlerCreateRangeExpenses))]
public class TestHandlerCreateRangeExpense
    : TestHandlerCreateRange<
        HandlerCreateRangeExpenses,
        Expense,
        InputDTOExpense,
        CommandCreateRangeExpenses,
        IRepositoryExpense,
        TestFactoryExpense
    >
{
    private readonly IList<InputDTOExpense> _inputDTOs =
    [
        new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test1",
            CategoryId = 1,
            PaymentMethodId = 1
        },
        new()
        {
            Amount = 200,
            Date = DateTime.Now,
            Description = "Test2",
            CategoryId = 2,
            PaymentMethodId = 2
        },
    ];

    protected override HandlerCreateRangeExpenses CreateHandler()
    {
        return new HandlerCreateRangeExpenses(
            GetMock<IRepositoryExpense>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override IEnumerable<InputDTOExpense> GetInputDTOs() => _inputDTOs;
    protected override CommandCreateRangeExpenses GetCommand() => new() { Data = _inputDTOs };

    protected override IList<Expense> GetMappedEntities() =>
        _inputDTOs.Select(dto => new Expense
        {
            Amount = dto.Amount,
            Date = dto.Date,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            PaymentMethodId = dto.PaymentMethodId,
            UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
        }).ToList();

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}