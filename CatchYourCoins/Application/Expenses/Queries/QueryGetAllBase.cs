using Domain;
using MediatR;

namespace Application.Expenses.Queries;

public class QueryGetAllBase<TDTO> : IRequest<Result<IReadOnlyList<TDTO>>>
{
    
}