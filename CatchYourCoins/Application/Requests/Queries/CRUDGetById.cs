using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Requests.Queries;

public class QueryCRUDGetById<TDTO> : IRequest<Result<TDTO>>
{
    public required int Id { get; init; }
}

public class HandlerCRUDGetById<TEntity, TDTO>(
    IRepositoryCRUD<TEntity> repository,
    IMapper mapper)
    : IRequestHandler<QueryCRUDGetById<TDTO>, Result<TDTO>>
{
    public async Task<Result<TDTO>> Handle(QueryCRUDGetById<TDTO> request, CancellationToken cancellationToken)
    {
        TEntity? entity = await repository.GetByIdAsync(request.Id);

        return entity == null
            ? Result<TDTO>.Failure(GetFailureMessages())
            : Result<TDTO>.SetValue(mapper.Map<TDTO>(entity));
    }
    
    private Dictionary<string, string> GetFailureMessages() => new()
    {
        { "GetById", "Entity not found" }
    };
}