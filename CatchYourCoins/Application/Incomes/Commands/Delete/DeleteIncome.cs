using Application.Requests.Commands;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Incomes.Commands.Delete;

public class CommandDeleteIncome : CommandCRUDDelete;

[UsedImplicitly]
public class ValidatorDeleteIncome : ValidatorCRUDDelete<CommandDeleteIncome>;

public class HandlerDeleteIncome(
    IRepositoryIncome repository,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<Income, CommandDeleteIncome>(repository, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Expense", "Expense not found" }
        };
}