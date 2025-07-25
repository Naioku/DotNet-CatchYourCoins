using Domain;
using MediatR;

namespace Application.Requests.Commands.Delete;

public class CommandDeleteBase : IRequest<Result>
{
    public required int Id { get; init; }
}