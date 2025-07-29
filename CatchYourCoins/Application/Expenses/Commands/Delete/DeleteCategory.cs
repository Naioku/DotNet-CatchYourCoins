using Application.Requests.Commands;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Delete;

public class CommandDeleteCategory : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeleteCategory : ValidatorDeleteBase<CommandDeleteCategory>;

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