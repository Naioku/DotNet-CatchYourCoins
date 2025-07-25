using Domain;
using MediatR;

namespace Application.Expenses.Queries;

public class QueryGetByIdBase<TDTO> : IRequest<Result<TDTO>>
{
    public required int Id { get; init; }
}