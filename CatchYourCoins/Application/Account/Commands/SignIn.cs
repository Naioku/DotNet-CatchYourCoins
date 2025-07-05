using Domain;
using Domain.IdentityEntities;
using Domain.Interfaces;

namespace Application.Account.Commands;

public class CommandSignIn
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class HandlerSignIn(IServiceIdentity identityService)
{
    public async Task<Result<ResultSignIn>> Handle(CommandSignIn command) => await identityService.SignIn(command.Email, command.Password);
}