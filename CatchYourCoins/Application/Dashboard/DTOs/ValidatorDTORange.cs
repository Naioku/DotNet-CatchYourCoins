using FluentValidation;

namespace Application.Dashboard.DTOs;

public class ValidatorDTORange<TDTO, TDTOValidator> : AbstractValidator<DTORange<TDTO>>
    where TDTOValidator : AbstractValidator<TDTO>, new()
{
    public ValidatorDTORange()
    {
        RuleForEach(x => x.Items)
            .NotNull()
            .SetValidator(new TDTOValidator());
    }
}