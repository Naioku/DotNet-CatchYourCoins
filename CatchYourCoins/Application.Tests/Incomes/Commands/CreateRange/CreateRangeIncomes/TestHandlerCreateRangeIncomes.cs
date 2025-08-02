using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Incomes;
using Application.Incomes.Commands.CreateRange;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Commands.CreateRange.CreateRangeIncomes;

[TestSubject(typeof(HandlerCreateRangeIncomes))]
public class TestHandlerCreateRangeIncomes
    : TestHandlerCreateRange<
        HandlerCreateRangeIncomes,
        Income,
        InputDTOIncome,
        CommandCreateRangeIncomes,
        IRepositoryIncome,
        TestFactoryIncome
    >
{
    private readonly IList<InputDTOIncome> _inputDTOs = [
        new()
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test1",
            CategoryId = 1,
        },
        new()
        {
            Amount = 200,
            Date = DateTime.Now,
            Description = "Test2",
            CategoryId = 2,
        },
    ];

    protected override HandlerCreateRangeIncomes CreateHandler()
    {
        return new HandlerCreateRangeIncomes(
            GetMock<IRepositoryIncome>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override IEnumerable<InputDTOIncome> GetInputDTOs() => _inputDTOs;
    protected override CommandCreateRangeIncomes GetCommand() => new() { Data = _inputDTOs };

    protected override IList<Income> GetMappedEntities() =>
        _inputDTOs.Select(dto => new Income
        {
            Amount = dto.Amount,
            Date = dto.Date,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
        }).ToList();

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}