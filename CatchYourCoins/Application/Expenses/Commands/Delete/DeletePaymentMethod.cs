using Application.Requests.Commands;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Delete;

public class CommandDeletePaymentMethod : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeletePaymentMethod : ValidatorDeleteBase<CommandDeletePaymentMethod>;

public class HandlerDeletePaymentMethod(
    IRepositoryExpensePaymentMethod repository,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<ExpensePaymentMethod, CommandDeletePaymentMethod>(repository, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethod", "Payment method not found" }
        };
}