using System;
using System.Threading.Tasks;
using Application.DTOs.InputDTOs.Incomes;
using Application.Incomes.Commands.Create;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Commands.Create.CreateIncome;

[TestSubject(typeof(HandlerCreateIncome))]
public class HandlerCreateIncomeTest
    : TestHandlerCreate<
        HandlerCreateIncome,
        Income,
        InputDTOIncome,
        CommandCreateIncome,
        IRepositoryIncome,
        TestFactoryIncome
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

    protected override InputDTOIncome GetInputDTO() => _dto;
    protected override CommandCreateIncome GetCommand() => new() { Data = _dto };

    protected override Income GetMappedEntity() => new()
    {
        Amount = _dto.Amount,
        Date = _dto.Date,
        Description = _dto.Description,
        CategoryId = _dto.CategoryId,
        UserId = TestFactoryUsers.DefaultUser1Authenticated.Id,
    };

    [Fact]
    public async Task Create_ValidData_EntityCreated() =>
        await Create_ValidData_EntityCreated_Base();
}