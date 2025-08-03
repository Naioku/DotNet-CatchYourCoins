using Application.DTOs.OutputDTOs.Expenses;
using Domain.Dashboard.Entities.Expenses;

namespace Application.Tests.Factories.DTOs.OutputDTO;

public class TestFactoryOutputDTOExpense : TestFactoryDTOBase<Expense, OutputDTOExpense>
{
    public override OutputDTOExpense CreateDTO(Expense entity) =>
        new()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Date = entity.Date,
            Description = entity.Description,
            Category = entity.Category?.Name,
            PaymentMethod = entity.PaymentMethod?.Name,
        };
}