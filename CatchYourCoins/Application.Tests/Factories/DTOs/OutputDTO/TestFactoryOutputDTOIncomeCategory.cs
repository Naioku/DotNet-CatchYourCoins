using Application.DTOs.OutputDTOs.Incomes;
using Domain.Dashboard.Entities.Incomes;

namespace Application.Tests.Factories.DTOs.OutputDTO;

public class TestFactoryOutputDTOIncomeCategory : TestFactoryDTOBase<IncomeCategory, OutputDTOIncomeCategory>
{
    public override OutputDTOIncomeCategory CreateDTO(IncomeCategory entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}