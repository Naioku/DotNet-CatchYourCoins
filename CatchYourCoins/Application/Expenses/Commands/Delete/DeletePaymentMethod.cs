using Application.Requests.Commands.Delete;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Application.Expenses.Commands.Delete;

public class CommandDeletePaymentMethod : CommandDeleteBase;

[UsedImplicitly]
public class TestValidatorDeletePaymentMethod : ValidatorDeleteBase<CommandDeletePaymentMethod>;

public class TestHandlerDeletePaymentMethod(
    IRepositoryPaymentMethod repositoryPaymentMethod,
    IUnitOfWork unitOfWork) : HandlerCRUDDelete<PaymentMethod, CommandDeletePaymentMethod>(repositoryPaymentMethod, unitOfWork)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "PaymentMethod", "Payment method not found" }
        };
}