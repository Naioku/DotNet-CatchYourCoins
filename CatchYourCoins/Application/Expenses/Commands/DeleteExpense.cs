using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands;

public class CommandDeleteExpense : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeleteExpense : ValidatorDeleteBase<CommandDeleteExpense>;

public class HandlerDeleteExpense(
    IRepositoryExpense repositoryExpense,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<Expense, CommandDeleteExpense>(repositoryExpense, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Expense", "Expense not found" }
        };
}