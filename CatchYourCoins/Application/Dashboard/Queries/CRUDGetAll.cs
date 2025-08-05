using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Dashboard.Queries;

public class QueryCRUDGetAll<TDTO> : IRequest<Result<IReadOnlyList<TDTO>>>;

public class HandlerCRUDGetAll<TEntity, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IMapper mapper)
    : IRequestHandler<QueryCRUDGetAll<TDTO>, Result<IReadOnlyList<TDTO>>>
{
    public async Task<Result<IReadOnlyList<TDTO>>> Handle(QueryCRUDGetAll<TDTO> _, CancellationToken cancellationToken)
    {
        IReadOnlyList<TEntity> entity = await repository.GetAllAsync();

        return !entity.Any()
            ? Result<IReadOnlyList<TDTO>>.Failure(GetFailureMessages())
            : Result<IReadOnlyList<TDTO>>.SetValue(mapper.Map<IReadOnlyList<TDTO>>(entity));
    }

    private Dictionary<string, string> GetFailureMessages() => new()
    {
        { "GetAll", "Entities not found" }
    };
}