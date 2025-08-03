using Application.DTOs.OutputDTOs.Incomes;
using Domain.Dashboard.Entities.Incomes;

namespace Application.Tests.Factories.DTOs.OutputDTO;

public class TestFactoryOutputDTOIncome : TestFactoryDTOBase<Income, OutputDTOIncome>
{
    public override OutputDTOIncome CreateDTO(Income entity) =>
        new()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Date = entity.Date,
            Description = entity.Description,
            Category = entity.Category?.Name,
        };
}