using System.ComponentModel.DataAnnotations;
using Domain;
using Domain.IdentityEntities;
using Domain.Interfaces.Services;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Account.Commands;

public class CommandLogIn : IRequest<Result<ResultLogIn>>
{
    [DataType(DataType.EmailAddress)]
    public required string Email { get; init; }
    
    [DataType(DataType.Password)]
    public required string Password { get; init; }
}

public class HandlerLogIn(IServiceIdentity identityService) : IRequestHandler<CommandLogIn, Result<ResultLogIn>>
{
    public async Task<Result<ResultLogIn>> Handle(CommandLogIn request, CancellationToken cancellationToken) =>
        await identityService.LogInAsync(request.Email, request.Password);
}

[UsedImplicitly]
public class ValidatorLogIn : AbstractValidator<CommandLogIn>
{
    public ValidatorLogIn()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}