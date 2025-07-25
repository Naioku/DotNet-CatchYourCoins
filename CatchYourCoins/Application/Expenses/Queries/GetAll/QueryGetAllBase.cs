using Domain;
using MediatR;

namespace Application.Expenses.Queries.GetAll;

public class QueryGetAllBase<TDTO> : IRequest<Result<IReadOnlyList<TDTO>>>
{
    
}