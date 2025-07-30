using Application.DTOs.InputDTOs;
using FluentValidation;

namespace Application.Requests.Commands;

public class ValidatorInputDTOFinancialOperation<TDTO> : AbstractValidator<TDTO>
    where TDTO : InputDTOFinancialOperation
{
    protected ValidatorInputDTOFinancialOperation()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo((decimal)9999999999999999.99); // decimal(18, 2)
        
        RuleFor(x => x.Date)
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .MinimumLength(1)
            .MaximumLength(8000);
    }
}