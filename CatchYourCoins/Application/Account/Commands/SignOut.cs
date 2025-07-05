using Domain.Interfaces;

namespace Application.Account.Commands;

public class HandlerSignOut(IServiceIdentity identityService)
{
    public async Task Handle() => await identityService.SignOut();
}