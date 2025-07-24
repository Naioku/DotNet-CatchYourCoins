using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands;

public class CommandDeleteExpense : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeleteExpense : ValidatorDeleteBase<CommandDeleteExpense>;

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