using Domain;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Requests.Commands.Delete;

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