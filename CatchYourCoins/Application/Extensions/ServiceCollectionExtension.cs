using Application.Account.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<HandlerRegister>();
        services.AddScoped<HandlerSignIn>();
        services.AddScoped<HandlerSignOut>();
        
        return services;
    }
}