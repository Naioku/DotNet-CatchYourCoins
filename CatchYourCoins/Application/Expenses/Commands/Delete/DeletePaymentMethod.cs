using Application.Requests.Commands.Delete;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Delete;

public class CommandDeletePaymentMethod : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeletePaymentMethod : ValidatorDeleteBase<CommandDeletePaymentMethod>;

public class HandlerDeletePaymentMethod(
    IRepositoryPaymentMethod repository,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<PaymentMethod, CommandDeletePaymentMethod>(repository, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethod", "Payment method not found" }
        };
}