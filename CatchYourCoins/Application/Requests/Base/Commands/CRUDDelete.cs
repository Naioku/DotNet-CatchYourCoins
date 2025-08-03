using Domain;
using Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Requests.Base.Commands;

public abstract class CommandCRUDDelete : IRequest<Result>
{
    public required int Id { get; init; }
}

public abstract class ValidatorCRUDDelete<T> : AbstractValidator<T> where T : CommandCRUDDelete
{
    protected ValidatorCRUDDelete()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0);
    }
}

public abstract class HandlerCRUDDelete<TEntity, TCommand>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<TCommand, Result>
    where TCommand : CommandCRUDDelete
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