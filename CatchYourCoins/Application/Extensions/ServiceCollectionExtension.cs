using Application.Account.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<CommandRegister>());
        
        return services;
    }
}