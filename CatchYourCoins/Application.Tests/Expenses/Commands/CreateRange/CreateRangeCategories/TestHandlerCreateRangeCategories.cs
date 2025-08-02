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

namespace Application.Tests.Expenses.Commands.CreateRange.CreateRangeCategories;

[TestSubject(typeof(HandlerCreateRangeCategories))]
public class TestHandlerCreateRangeCategories
    : TestHandlerCreateRange<
        HandlerCreateRangeCategories,
        ExpenseCategory,
        InputDTOExpenseCategory,
        CommandCreateRangeCategories,
        IRepositoryExpenseCategory,
        TestFactoryExpenseCategory
    >
{
    private readonly IList<InputDTOExpenseCategory> _inputDTOs =
    [
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

    protected override HandlerCreateRangeCategories CreateHandler()
    {
        return new HandlerCreateRangeCategories(
            GetMock<IRepositoryExpenseCategory>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override IEnumerable<InputDTOExpenseCategory> GetInputDTOs() => _inputDTOs;
    protected override CommandCreateRangeCategories GetCommand() => new() { Data = _inputDTOs };

    protected override IList<ExpenseCategory> GetMappedEntities() =>
        _inputDTOs.Select(inputDTO => new ExpenseCategory
        {
            Name = inputDTO.Name,
            Limit = inputDTO.Limit,
            UserId = TestFactoryUsers.DefaultUser1Authenticated.Id
        }).ToList();

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}