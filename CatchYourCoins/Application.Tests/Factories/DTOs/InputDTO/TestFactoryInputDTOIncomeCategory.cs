using Application.DTOs.InputDTOs.Incomes;
using Domain.Dashboard.Entities.Incomes;

namespace Application.Tests.Factories.DTOs.InputDTO;

public class TestFactoryInputDTOIncomeCategory : TestFactoryDTOBase<IncomeCategory, InputDTOIncomeCategory>
{
    public override InputDTOIncomeCategory CreateDTO(IncomeCategory entity) =>
        new()
        {
            Name = entity.Name,
            Limit = entity.Limit,
        };
}