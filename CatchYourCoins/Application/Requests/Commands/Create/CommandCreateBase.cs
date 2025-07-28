using Domain;
using MediatR;

namespace Application.Requests.Commands.Create;

public class CommandCreateBase : IRequest<Result>;