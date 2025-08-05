using FluentValidation;

namespace Application.DTOs.InputDTOs;

public class ValidatorInputDTOFinancialOperation<TDTO> : AbstractValidator<TDTO>
    where TDTO : InputDTOFinancialOperation
{
    public ValidatorInputDTOFinancialOperation()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(9999999999999999.99m); // decimal(18, 2)
        
        RuleFor(x => x.Date)
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .MinimumLength(1)
            .MaximumLength(8000);
    }
}