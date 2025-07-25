using Application.Account.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ExtensionServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<CommandRegister>();
        });

        services.AddValidatorsFromAssemblyContaining<ValidatorRegister>();
        
        return services;
    }
}