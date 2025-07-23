using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands;

public class CommandDeleteExpense : IRequest<Result>
{
    public required int Id { get; init; }
}

[UsedImplicitly]
public class ValidatorDeleteExpense : AbstractValidator<CommandDeleteExpense>
{
    public ValidatorDeleteExpense()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0);
    }
}

public class HandlerDeleteExpense(
    IRepositoryExpense repositoryExpense,
    IUnitOfWork unitOfWork) : IRequestHandler<CommandDeleteExpense, Result>
{
    public async Task<Result> Handle(CommandDeleteExpense request, CancellationToken cancellationToken)
    {
        Expense? expense = await repositoryExpense.GetExpenseByIdAsync(request.Id);
        if (expense == null)
        {
            return Result.Failure(new Dictionary<string, string>()
            {
                {"Expense", "Expense not found"}
            });
        }
        
        repositoryExpense.DeleteExpense(expense);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}