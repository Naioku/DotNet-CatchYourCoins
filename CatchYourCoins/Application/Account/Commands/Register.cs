using Domain;
using Domain.Interfaces;

namespace Application.Account.Commands;

public class CommandRegister
{
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}

public class HandlerRegister(IServiceIdentity identityService)
{
    public async Task<Result> Handle(CommandRegister command)
    {
        return await identityService.RegisterUserAsync(command.Email, command.UserName, command.Password);
    }
}