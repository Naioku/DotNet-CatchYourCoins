using Application.DTOs.OutputDTOs.Expenses;
using Domain.Dashboard.Entities.Expenses;

namespace Application.Tests.Factories.DTOs.OutputDTO;

public class TestFactoryOutputDTOExpenseCategory : TestFactoryDTOBase<ExpenseCategory, OutputDTOExpenseCategory>
{
    public override OutputDTOExpenseCategory CreateDTO(ExpenseCategory entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}