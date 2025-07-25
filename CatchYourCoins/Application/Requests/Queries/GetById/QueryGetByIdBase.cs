using Domain;
using MediatR;

namespace Application.Requests.Queries.GetById;

public abstract class QueryGetByIdBase<TDTO> : IRequest<Result<TDTO>>
{
    public required int Id { get; init; }
}