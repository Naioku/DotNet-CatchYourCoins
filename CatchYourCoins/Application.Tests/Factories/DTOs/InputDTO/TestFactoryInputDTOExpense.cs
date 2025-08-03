using Application.DTOs.InputDTOs.Expenses;
using Domain.Dashboard.Entities.Expenses;

namespace Application.Tests.Factories.DTOs.InputDTO;

public class TestFactoryInputDTOExpense : TestFactoryDTOBase<Expense, InputDTOExpense>
{
    public override InputDTOExpense CreateDTO(Expense entity) =>
        new()
        {
            Amount = entity.Amount,
            Date = entity.Date,
            Description = entity.Description,
            CategoryId = entity.Category?.Id,
            PaymentMethodId = entity.PaymentMethod?.Id,
        };
}