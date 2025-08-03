using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Incomes.Commands.CreateRange;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Incomes.Commands.CreateRange.CreateRangeIncomes;

[TestSubject(typeof(HandlerCreateRangeIncomes))]
public class TestHandlerCreateRangeIncomes
    : TestHandlerCreateRange<
        HandlerCreateRangeIncomes,
        Income,
        InputDTOIncome,
        CommandCreateRangeIncomes,
        IRepositoryIncome
    >
{
    protected override HandlerCreateRangeIncomes CreateHandler()
    {
        return new HandlerCreateRangeIncomes(
            GetMock<IRepositoryIncome>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override CommandCreateRangeIncomes GetCommand(List<InputDTOIncome> dtos) => new() { Data = dtos };

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