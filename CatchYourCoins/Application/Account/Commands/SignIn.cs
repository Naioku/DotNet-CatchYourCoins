using Domain;
using Domain.IdentityEntities;
using Domain.Interfaces;
using MediatR;

namespace Application.Account.Commands;

public class CommandSignIn : IRequest<Result<ResultSignIn>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class HandlerSignIn(IServiceIdentity identityService) : IRequestHandler<CommandSignIn, Result<ResultSignIn>>
{
    public async Task<Result<ResultSignIn>> Handle(CommandSignIn request, CancellationToken cancellationToken) =>
        await identityService.SignIn(request.Email, request.Password);
}