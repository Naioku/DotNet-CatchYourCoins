using Domain;
using MediatR;

namespace Application.Expenses.Queries;

public abstract class QueryGetByIdBase<TDTO> : IRequest<Result<TDTO>>
{
    public required int Id { get; init; }
}