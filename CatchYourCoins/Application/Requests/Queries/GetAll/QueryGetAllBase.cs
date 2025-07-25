using Domain;
using MediatR;

namespace Application.Requests.Queries.GetAll;

public class QueryGetAllBase<TDTO> : IRequest<Result<IReadOnlyList<TDTO>>>;