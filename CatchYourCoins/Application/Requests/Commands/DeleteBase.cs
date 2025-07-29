using Domain;
using Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Requests.Commands;

public class CommandDeleteBase : IRequest<Result>
{
    public required int Id { get; init; }
}

public abstract class ValidatorDeleteBase<T> : AbstractValidator<T> where T : CommandDeleteBase
{
    protected ValidatorDeleteBase()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0);
    }
}

public abstract class HandlerCRUDDelete<TEntity, TCommand>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<TCommand, Result>
    where TCommand : CommandDeleteBase
{
    protected abstract Dictionary<string, string> GetFailureMessages();

    public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
    {
        TEntity? expense = await repository.GetByIdAsync(request.Id);
        if (expense == null)
        {
            return Result.Failure(GetFailureMessages());
        }
        
        repository.Delete(expense);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}