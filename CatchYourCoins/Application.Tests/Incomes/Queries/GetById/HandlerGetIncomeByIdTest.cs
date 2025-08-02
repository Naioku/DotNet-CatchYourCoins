using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Incomes;
using Application.Incomes.Queries.GetById;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Queries.GetById;

[TestSubject(typeof(HandlerGetIncomeById))]
public class HandlerGetIncomeByIdTest
    : TestHandlerGetById<
        HandlerGetIncomeById,
        Income,
        OutputDTOIncome,
        QueryGetIncomeById,
        IRepositoryIncome,
        TestFactoryIncome
    >
{
    protected override HandlerGetIncomeById CreateHandler() =>
        new(
            GetMock<IRepositoryIncome>().Object,
            GetMock<IMapper>().Object
        );

    protected override QueryGetIncomeById GetQuery() => new() { Id = 1 };

    protected override OutputDTOIncome GetMappedDTO(Income entity) =>
        new()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Date = entity.Date,
            Description = entity.Description,
            Category = entity.Category?.Name,
        };

    [Fact]
    public async Task GetOne_ValidData_ReturnedOne() =>
        await GetOne_ValidData_ReturnedOne_Base();

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnedNull() =>
        await GetOne_NoEntryAtPassedID_ReturnedNull_Base();
}