using Domain;
using MediatR;

namespace Application.Expenses.Commands;

public class CommandDeleteBase : IRequest<Result>
{
    public required int Id { get; init; }
}