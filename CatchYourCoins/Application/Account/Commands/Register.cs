using System.ComponentModel.DataAnnotations;
using Domain;
using Domain.Interfaces;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;

namespace Application.Account.Commands;

public class CommandRegister : IRequest<Result>
{
    [DataType(DataType.EmailAddress)]
    public required string Email { get; init; }
    public required string UserName { get; init; }
    
    [DataType(DataType.Password)]
    public required string Password { get; init; }
    
    [DataType(DataType.Password)]
    public required string PasswordConfirmation { get; init; }
}

public class HandlerRegister(IServiceIdentity identityService) : IRequestHandler<CommandRegister, Result>
{
    public async Task<Result> Handle(CommandRegister request, CancellationToken cancellationToken) =>
        await identityService.RegisterUserAsync(request.Email, request.UserName, request.Password);
}

[UsedImplicitly]
public class ValidatorRegister : AbstractValidator<CommandRegister>
{
    public ValidatorRegister()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(x => x.UserName)
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .NotEmpty();
        
        RuleFor(x => x.PasswordConfirmation)
            .NotEmpty()
            .Equal(x => x.Password);
    }
}