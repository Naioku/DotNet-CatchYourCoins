using Domain.Interfaces.Services;
using MediatR;

namespace Application.Account.Commands;

public class CommandSignOut : IRequest;

public class HandlerSignOut(IServiceIdentity identityService) : IRequestHandler<CommandSignOut>
{
    public async Task Handle(CommandSignOut request, CancellationToken cancellationToken) => await identityService.SignOut();
}