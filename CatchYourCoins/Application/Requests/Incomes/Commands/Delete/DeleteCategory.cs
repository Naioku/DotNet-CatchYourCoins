using Application.Requests.Base.Commands;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Requests.Incomes.Commands.Delete;

public class CommandDeleteCategory : CommandCRUDDelete;

[UsedImplicitly]
public class ValidatorDeleteCategory : ValidatorCRUDDelete<CommandDeleteCategory>;

public class HandlerDeleteCategory(
    IRepositoryIncomeCategory repository,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<IncomeCategory, CommandDeleteCategory>(repository, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };
}