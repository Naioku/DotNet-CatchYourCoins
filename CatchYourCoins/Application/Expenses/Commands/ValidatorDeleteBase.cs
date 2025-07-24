using FluentValidation;

namespace Application.Expenses.Commands;

public abstract class ValidatorDeleteBase<T> : AbstractValidator<T> where T : CommandDeleteBase
{
    protected ValidatorDeleteBase()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0);
    }
}