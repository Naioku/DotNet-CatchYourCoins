using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands;

public class CommandDeleteCategory : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeleteCategory : ValidatorDeleteBase<CommandDeleteCategory>;

public class HandlerDeleteCategory(
    IRepositoryCategory repositoryCategory,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<Category, CommandDeleteCategory>(repositoryCategory, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };
}