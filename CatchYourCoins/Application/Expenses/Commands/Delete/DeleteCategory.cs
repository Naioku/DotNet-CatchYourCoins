using Application.Requests.Commands.Delete;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Delete;

public class CommandDeleteCategory : CommandDeleteBase;

[UsedImplicitly]
public class TestValidatorDeleteCategory : ValidatorDeleteBase<CommandDeleteCategory>;

public class TestHandlerDeleteCategory(
    IRepositoryCategory repositoryCategory,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<Category, CommandDeleteCategory>(repositoryCategory, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };
}