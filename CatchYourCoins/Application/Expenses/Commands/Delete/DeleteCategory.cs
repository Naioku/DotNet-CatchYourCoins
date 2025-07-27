using Application.Requests.Commands.Delete;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Delete;

public class CommandDeleteCategory : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeleteCategory : ValidatorDeleteBase<CommandDeleteCategory>;

public class HandlerDeleteCategory(
    IRepositoryCategoryExpenses repositoryCategory,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<CategoryExpenses, CommandDeleteCategory>(repositoryCategory, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };
}