using Application.Requests.Base.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Requests.Expenses.Commands.Delete;

public class CommandDeleteExpense : CommandCRUDDelete;

[UsedImplicitly]
public class ValidatorDeleteExpense : ValidatorCRUDDelete<CommandDeleteExpense>;

public class HandlerDeleteExpense(
    IRepositoryExpense repository,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<Expense, CommandDeleteExpense>(repository, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Expense", "Expense not found" }
        };
}