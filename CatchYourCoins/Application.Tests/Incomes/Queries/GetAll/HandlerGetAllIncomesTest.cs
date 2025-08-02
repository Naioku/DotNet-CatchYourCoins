using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Incomes;
using Application.Incomes.Queries.GetAll;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllIncomes))]
public class HandlerGetAllIncomesTest
    : TestHandlerGetAll<
        HandlerGetAllIncomes,
        Income,
        OutputDTOIncome,
        QueryGetAllIncomes,
        IRepositoryIncome,
        TestFactoryIncome
    >
{
    protected override HandlerGetAllIncomes CreateHandler() =>
        new(
            GetMock<IRepositoryIncome>().Object,
            GetMock<IMapper>().Object
        );

    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base();

    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();

    protected override IReadOnlyList<OutputDTOIncome> GetMappedDTOs(List<Income> entity) =>
        entity.Select(e => new OutputDTOIncome
        {
            Id = e.Id,
            Amount = e.Amount,
            Date = e.Date,
            Description = e.Description,
            Category = e.Category?.Name,
        }).ToList();
}