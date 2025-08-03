using Application.DTOs.InputDTOs.Incomes;
using Domain.Dashboard.Entities.Incomes;

namespace Application.Tests.Factories.DTOs.InputDTO;

public class TestFactoryInputDTOIncome : TestFactoryDTOBase<Income, InputDTOIncome>
{
    public override InputDTOIncome CreateDTO(Income entity) =>
        new()
        {
            Amount = entity.Amount,
            Date = entity.Date,
            Description = entity.Description,
            CategoryId = entity.Category?.Id,
        };
}