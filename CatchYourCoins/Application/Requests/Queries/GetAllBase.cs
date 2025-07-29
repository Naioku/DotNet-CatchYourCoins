using Domain;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Requests.Queries;

public class QueryGetAllBase<TDTO> : IRequest<Result<IReadOnlyList<TDTO>>>;

public abstract class HandlerCRUDGetAll<TEntity, TQuery, TDTO>(IRepositoryCRUD<TEntity> repository)
    : IRequestHandler<TQuery, Result<IReadOnlyList<TDTO>>>
    where TQuery : QueryGetAllBase<TDTO>
{
    protected abstract Dictionary<string, string> GetFailureMessages();
    protected abstract TDTO MapEntityToDTO(TEntity entity);
    
    public async Task<Result<IReadOnlyList<TDTO>>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<TEntity> entity = await repository.GetAllAsync();

        return !entity.Any()
            ? Result<IReadOnlyList<TDTO>>.Failure(GetFailureMessages())
            : Result<IReadOnlyList<TDTO>>.SetValue(entity.Select(MapEntityToDTO).ToList());
    }
}