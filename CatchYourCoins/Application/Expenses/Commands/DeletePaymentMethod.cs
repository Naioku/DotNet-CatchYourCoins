using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands;

public class CommandDeletePaymentMethod : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeletePaymentMethod : ValidatorDeleteBase<CommandDeletePaymentMethod>;

public class HandlerDeletePaymentMethod(
    IRepositoryPaymentMethod repositoryPaymentMethod,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<PaymentMethod, CommandDeletePaymentMethod>(repositoryPaymentMethod, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethod", "Payment method not found" }
        };
}