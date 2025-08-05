using Domain;
using Domain.Interfaces.Repositories;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Requests.Commands;

public class CommandCRUDDelete<TEntity> : IRequest<Result>
{
    public required int Id { get; init; }
}

[UsedImplicitly]
public class ValidatorCRUDDelete<TEntity> : AbstractValidator<CommandCRUDDelete<TEntity>>
{
    public ValidatorCRUDDelete()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0);
    }
}

public class HandlerCRUDDelete<TEntity>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CommandCRUDDelete<TEntity>, Result>
{
    public async Task<Result> Handle(CommandCRUDDelete<TEntity> request, CancellationToken cancellationToken)
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

    private Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Delete", "Entity not found" }
        };
}