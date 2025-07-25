using Domain;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Application.Expenses.Commands;

public class CommandDeletePaymentMethod : CommandDeleteBase;

[UsedImplicitly]
public class ValidatorDeletePaymentMethod : ValidatorDeleteBase<CommandDeletePaymentMethod>;

public class HandlerDeletePaymentMethod(
    IRepositoryPaymentMethod repositoryPaymentMethod,
    IUnitOfWork unitOfWork) : IRequestHandler<CommandDeletePaymentMethod, Result>
{
    public async Task<Result> Handle(CommandDeletePaymentMethod request, CancellationToken cancellationToken)
    {
        PaymentMethod? expense = await repositoryPaymentMethod.GetByIdAsync(request.Id);
        if (expense == null)
        {
            return Result.Failure(new Dictionary<string, string>()
            {
                {"PaymentMethod", "Payment method not found"}
            });
        }
        
        repositoryPaymentMethod.Delete(expense);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}