using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Requests.Commands.Create;

public abstract class HandlerCRUDCreate<TEntity, TCommand>(
    IRepositoryCRUD<TEntity> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<TCommand>
where TCommand : IRequest
{
    protected abstract TEntity MapCommandToEntity(TCommand request);

    public async Task Handle(TCommand request, CancellationToken cancellationToken)
    {
        await repository.CreateAsync(MapCommandToEntity(request));
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}