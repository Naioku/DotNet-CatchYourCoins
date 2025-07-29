using Application.Requests.Commands.Delete;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Delete;

public class CommandDeleteExpense : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeleteExpense : ValidatorDeleteBase<CommandDeleteExpense>;

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