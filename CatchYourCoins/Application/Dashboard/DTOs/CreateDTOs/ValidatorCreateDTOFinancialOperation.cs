using FluentValidation;

namespace Application.Dashboard.DTOs.CreateDTOs;

public class ValidatorCreateDTOFinancialOperation<TDTO> : AbstractValidator<TDTO>
    where TDTO : CreateDTOFinancialOperation
{
    public ValidatorCreateDTOFinancialOperation()
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