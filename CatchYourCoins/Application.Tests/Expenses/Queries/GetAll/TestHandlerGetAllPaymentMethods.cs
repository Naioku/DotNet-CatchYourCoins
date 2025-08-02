using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Expenses.Queries.GetAll;
using Application.Tests.Factories;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetAll;

[TestSubject(typeof(HandlerGetAllPaymentMethods))]
public class TestHandlerGetAllPaymentMethods
    : TestHandlerGetAll<
        HandlerGetAllPaymentMethods,
        ExpensePaymentMethod,
        OutputDTOExpensePaymentMethod,
        QueryGetAllPaymentMethods,
        IRepositoryExpensePaymentMethod,
        TestFactoryExpensePaymentMethod
    >
{
    protected override HandlerGetAllPaymentMethods CreateHandler() =>
        new(
            GetMock<IRepositoryExpensePaymentMethod>().Object,
            GetMock<IMapper>().Object
        );

    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base();

    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();

    protected override IReadOnlyList<OutputDTOExpensePaymentMethod> GetMappedDTOs(List<ExpensePaymentMethod> entity) =>
        entity.Select(e => new OutputDTOExpensePaymentMethod
        {
            Id = e.Id,
            Name = e.Name,
            Limit = e.Limit,
        }).ToList();
}