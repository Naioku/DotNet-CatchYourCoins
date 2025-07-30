using System.Threading.Tasks;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Expenses.Queries.GetById;
using Application.Tests.Factories;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Queries.GetById;

[TestSubject(typeof(HandlerGetPaymentMethodById))]
public class TestHandlerGetPaymentMethodById
    : TestHandlerGetById<
        HandlerGetPaymentMethodById,
        ExpensePaymentMethod,
        OutputDTOPaymentMethod,
        QueryGetPaymentMethodById,
        IRepositoryExpensePaymentMethod,
        TestFactoryPaymentMethod
    >
{
    protected override HandlerGetPaymentMethodById CreateHandler() =>
        new(GetMock<IRepositoryExpensePaymentMethod>().Object);
    
    protected override QueryGetPaymentMethodById GetQuery() => new() { Id = 1 };

    [Fact]
    public async Task GetOne_ValidData_ReturnedOne()
    {
        await GetOne_ValidData_ReturnedOne_Base((inputEntity, resultDTO) =>
        {
            Assert.Equal(inputEntity.Id, resultDTO.Id);
            Assert.Equal(inputEntity.Name, resultDTO.Name);
            Assert.Equal(inputEntity.Limit, resultDTO.Limit);
        });
    }

    [Fact]
    public async Task GetOne_NoEntryAtPassedID_ReturnedNull() =>
        await GetOne_NoEntryAtPassedID_ReturnedNull_Base();
}