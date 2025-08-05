using FluentValidation;

namespace Application.DTOs.InputDTOs;

public class ValidatorInputDTOFinancialCategory<TDTO> : AbstractValidator<TDTO>
    where TDTO : InputDTOFinancialCategory
{
    public ValidatorInputDTOFinancialCategory()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo((decimal)9999999999999999.99); // decimal(18, 2)
    }
}