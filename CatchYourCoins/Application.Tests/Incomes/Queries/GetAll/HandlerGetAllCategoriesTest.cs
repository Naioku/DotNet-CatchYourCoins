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

[TestSubject(typeof(HandlerGetAllCategories))]
public class HandlerGetAllCategoriesTest
    : TestHandlerGetAll<
        HandlerGetAllCategories,
        IncomeCategory,
        OutputDTOIncomeCategory,
        QueryGetAllCategories,
        IRepositoryIncomeCategory,
        TestFactoryIncomeCategory
    >
{
    protected override HandlerGetAllCategories CreateHandler() =>
        new(
            GetMock<IRepositoryIncomeCategory>().Object,
            GetMock<IMapper>().Object
        );

    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base();

    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();

    protected override IReadOnlyList<OutputDTOIncomeCategory> GetMappedDTOs(List<IncomeCategory> entity) =>
        entity.Select(e => new OutputDTOIncomeCategory
        {
            Id = e.Id,
            Name = e.Name,
            Limit = e.Limit,
        }).ToList();
}