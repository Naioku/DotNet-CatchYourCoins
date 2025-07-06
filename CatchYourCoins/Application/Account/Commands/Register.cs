using Domain;
using Domain.Interfaces;
using MediatR;

namespace Application.Account.Commands;

public class CommandRegister : IRequest<Result>
{
    public required string Email { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
}

public class HandlerRegister(IServiceIdentity identityService) : IRequestHandler<CommandRegister, Result>
{
    public async Task<Result> Handle(CommandRegister request, CancellationToken cancellationToken) =>
        await identityService.RegisterUserAsync(request.Email, request.UserName, request.Password);
}