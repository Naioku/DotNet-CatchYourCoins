using Domain;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Requests.Queries;

public abstract class QueryCRUDGetById<TDTO> : IRequest<Result<TDTO>>
{
    public required int Id { get; init; }
}

public abstract class HandlerCRUDGetById<TEntity, TQuery, TDTO>(IRepositoryCRUD<TEntity> repository)
    : IRequestHandler<TQuery, Result<TDTO>>
    where TQuery : QueryCRUDGetById<TDTO>
{
    protected abstract Dictionary<string, string> GetFailureMessages();
    protected abstract TDTO MapEntityToDTO(TEntity entity);

    public async Task<Result<TDTO>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        TEntity? entity = await repository.GetByIdAsync(request.Id);

        return entity == null
            ? Result<TDTO>.Failure(GetFailureMessages())
            : Result<TDTO>.SetValue(MapEntityToDTO(entity));
    }
}