using Application.Requests.Commands.Delete;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Delete;

public class CommandDeleteExpense : CommandDeleteBase;

[UsedImplicitly]
public class TestValidatorDeleteExpense : ValidatorDeleteBase<CommandDeleteExpense>;

public class TestHandlerDeleteExpense(
    IRepositoryExpense repositoryExpense,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<Expense, CommandDeleteExpense>(repositoryExpense, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Expense", "Expense not found" }
        };
}