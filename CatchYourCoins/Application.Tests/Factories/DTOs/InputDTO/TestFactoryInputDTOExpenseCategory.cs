using Application.DTOs.InputDTOs.Expenses;
using Domain.Dashboard.Entities.Expenses;

namespace Application.Tests.Factories.DTOs.InputDTO;

public class TestFactoryInputDTOExpenseCategory : TestFactoryDTOBase<ExpenseCategory, InputDTOExpenseCategory>
{
    public override InputDTOExpenseCategory CreateDTO(ExpenseCategory entity) =>
        new()
        {
            Name = entity.Name,
            Limit = entity.Limit,
        };
}