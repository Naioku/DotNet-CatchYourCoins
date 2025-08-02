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

[TestSubject(typeof(HandlerGetAllExpenses))]
public class TestHandlerGetAllExpenses
    : TestHandlerGetAll<
        HandlerGetAllExpenses,
        Expense,
        OutputDTOExpense,
        QueryGetAllExpenses,
        IRepositoryExpense,
        TestFactoryExpense
    >
{
    protected override HandlerGetAllExpenses CreateHandler() =>
        new(
            GetMock<IRepositoryExpense>().Object,
            GetMock<IMapper>().Object
        );

    [Fact]
    public async Task GetAll_ValidData_ReturnedAll() =>
        await GetAll_ValidData_ReturnedAll_Base();

    [Fact]
    public async Task GetAll_NoEntryInDB_ReturnedNull() =>
        await GetAll_NoEntryInDB_ReturnedNull_Base();

    protected override IReadOnlyList<OutputDTOExpense> GetMappedDTOs(List<Expense> entity) =>
        entity.Select(e => new OutputDTOExpense
        {
            Id = e.Id,
            Amount = e.Amount,
            Date = e.Date,
            Description = e.Description,
            Category = e.Category?.Name,
            PaymentMethod = e.PaymentMethod?.Name,
        }).ToList();
}