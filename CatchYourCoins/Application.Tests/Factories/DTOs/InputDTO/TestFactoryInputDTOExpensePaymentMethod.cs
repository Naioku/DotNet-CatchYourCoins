using Application.DTOs.InputDTOs.Expenses;
using Domain.Dashboard.Entities.Expenses;

namespace Application.Tests.Factories.DTOs.InputDTO;

public class TestFactoryInputDTOExpensePaymentMethod : TestFactoryDTOBase<ExpensePaymentMethod, InputDTOExpensePaymentMethod>
{
    public override InputDTOExpensePaymentMethod CreateDTO(ExpensePaymentMethod entity) =>
        new()
        {
            Name = entity.Name,
            Limit = entity.Limit,
        };
}