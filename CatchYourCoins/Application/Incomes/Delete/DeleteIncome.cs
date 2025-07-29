using Application.Requests.Commands.Delete;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Incomes.Delete;

public class CommandDeleteIncome : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeleteIncome : ValidatorDeleteBase<CommandDeleteIncome>;

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