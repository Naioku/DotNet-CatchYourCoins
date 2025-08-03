using Application.Requests.Base.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Requests.Expenses.Commands.Delete;

public class CommandDeleteCategory : CommandCRUDDelete;

[UsedImplicitly]
public class ValidatorDeleteCategory : ValidatorCRUDDelete<CommandDeleteCategory>;

public class HandlerDeleteCategory(
    IRepositoryExpenseCategory repository,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<ExpenseCategory, CommandDeleteCategory>(repository, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };
}