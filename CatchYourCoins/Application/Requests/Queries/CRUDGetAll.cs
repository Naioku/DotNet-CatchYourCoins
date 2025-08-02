using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Requests.Queries;

public abstract class QueryCRUDGetAll<TDTO> : IRequest<Result<IReadOnlyList<TDTO>>>;

public abstract class HandlerCRUDGetAll<TEntity, TQuery, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IMapper mapper)
    : IRequestHandler<TQuery, Result<IReadOnlyList<TDTO>>>
    where TQuery : QueryCRUDGetAll<TDTO>
{
    protected abstract Dictionary<string, string> GetFailureMessages();
    
    public async Task<Result<IReadOnlyList<TDTO>>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<TEntity> entity = await repository.GetAllAsync();

        return !entity.Any()
            ? Result<IReadOnlyList<TDTO>>.Failure(GetFailureMessages())
            : Result<IReadOnlyList<TDTO>>.SetValue(mapper.Map<IReadOnlyList<TDTO>>(entity));
    }
}