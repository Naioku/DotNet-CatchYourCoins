using Application.Requests.Commands.Delete;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Incomes.Delete;

public class CommandDeleteCategory : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeleteCategory : ValidatorDeleteBase<CommandDeleteCategory>;

public class HandlerDeleteCategory(
    IRepositoryCategoryIncomes repository,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<CategoryIncomes, CommandDeleteCategory>(repository, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };
}