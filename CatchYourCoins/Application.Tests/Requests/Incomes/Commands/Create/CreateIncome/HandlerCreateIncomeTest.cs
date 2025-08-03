using System;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Incomes.Commands.Create;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Incomes.Commands.Create.CreateIncome;

[TestSubject(typeof(HandlerCreateIncome))]
public class HandlerCreateIncomeTest
    : TestHandlerCreate<
        HandlerCreateIncome,
        Income,
        InputDTOIncome,
        CommandCreateIncome,
        IRepositoryIncome
    >
{
    private readonly InputDTOIncome _dto = new()
    {
        Amount = 100,
        Date = DateTime.Now,
        Description = "Test",
        CategoryId = 1,
    };

    protected override HandlerCreateIncome CreateHandler()
    {
        return new HandlerCreateIncome(
            GetMock<IRepositoryIncome>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );
    }

    protected override CommandCreateIncome GetCommand(InputDTOIncome dto) => new() { Data = dto };

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