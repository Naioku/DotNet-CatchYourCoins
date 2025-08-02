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

namespace Application.Tests.Incomes.Commands.CreateRange.CreateRangeCategories;

[TestSubject(typeof(HandlerCreateRangeCategories))]
public class TestHandlerCreateRangeCategories
    : TestHandlerCreateRange<
        HandlerCreateRangeCategories,
        IncomeCategory,
        InputDTOIncomeCategory,
        CommandCreateRangeCategories,
        IRepositoryIncomeCategory,
        TestFactoryIncomeCategory
    >
{
    private readonly IList<InputDTOIncomeCategory> _inputDTOs = [
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
            GetMock<IRepositoryIncomeCategory>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override IEnumerable<InputDTOIncomeCategory> GetInputDTOs() => _inputDTOs;
    protected override CommandCreateRangeCategories GetCommand() => new() { Data = _inputDTOs };

    protected override IList<IncomeCategory> GetMappedEntities() =>
        _inputDTOs.Select(dto => new IncomeCategory
        {
            Name = dto.Name,
            Limit = dto.Limit,
            UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
        }).ToList();

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}