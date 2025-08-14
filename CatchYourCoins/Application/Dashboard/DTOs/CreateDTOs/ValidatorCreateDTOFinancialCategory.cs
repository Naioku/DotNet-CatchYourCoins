using FluentValidation;

namespace Application.Dashboard.DTOs.CreateDTOs;

public class ValidatorCreateDTOFinancialCategory<TDTO> : AbstractValidator<TDTO>
    where TDTO : CreateDTOFinancialCategory
{
    public ValidatorCreateDTOFinancialCategory()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo((decimal)9999999999999999.99); // decimal(18, 2)
    }
}