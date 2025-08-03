using Application.DTOs.OutputDTOs.Expenses;
using Domain.Dashboard.Entities.Expenses;

namespace Application.Tests.Factories.DTOs.OutputDTO;

public class TestFactoryOutputDTOExpensePaymentMethod : TestFactoryDTOBase<ExpensePaymentMethod, OutputDTOExpensePaymentMethod>
{
    public override OutputDTOExpensePaymentMethod CreateDTO(ExpensePaymentMethod entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}