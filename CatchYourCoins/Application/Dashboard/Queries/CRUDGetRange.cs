using AutoMapper;
using Domain;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Specifications;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Dashboard.Queries;

public class QueryCRUDGet<TEntity,TDTO> : IRequest<Result<IReadOnlyList<TDTO>>>
    where TEntity : DashboardEntity
{
    public required ISpecificationDashboardEntity<TEntity> Specification { get; init; }
}

public class HandlerCRUDGet<TEntity, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IMapper mapper)
    : IRequestHandler<QueryCRUDGet<TEntity, TDTO>, Result<IReadOnlyList<TDTO>>>
    where TEntity : DashboardEntity
{
    public async Task<Result<IReadOnlyList<TDTO>>> Handle(QueryCRUDGet<TEntity, TDTO> request, CancellationToken cancellationToken)
    {
        IReadOnlyList<TEntity> entity = await repository.GetAsync(request.Specification, cancellationToken);

        return !entity.Any()
            ? Result<IReadOnlyList<TDTO>>.Failure(GetFailureMessages())
            : Result<IReadOnlyList<TDTO>>.SetValue(mapper.Map<IReadOnlyList<TDTO>>(entity));
    }

    private Dictionary<string, string> GetFailureMessages() => new()
    {
        { "GetByIdRange", "Entities not found" }
    };
}