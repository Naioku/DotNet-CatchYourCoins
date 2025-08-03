using Application.Account.Commands;
using Application.MappingProfiles;
using Application.MappingProfiles.Expenses;
using AutoMapper;
using Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Extensions;

public static class ExtensionServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => { config.RegisterServicesFromAssemblyContaining<CommandRegister>(); });

        services.AddScoped(provider => new MapperConfiguration(config =>
            {
                using (IServiceScope scope = provider.CreateScope())
                {
                    IServiceCurrentUser serviceCurrentUser = scope.ServiceProvider.GetRequiredService<IServiceCurrentUser>();
                    config.AddProfile(new MappingProfileFinancialCategory(serviceCurrentUser));
                    config.AddProfile(new MappingProfileFinancialOperation(serviceCurrentUser));
                    config.AddProfile(new MappingProfileExpense());
                }
            },
                provider.GetService<ILoggerFactory>()).CreateMapper()
        );

        // services.AddSingleton<Profile, MappingProfileFinancialCategory>();
        // services.AddSingleton<Profile, MappingProfileFinancialOperation>();
        // services.AddSingleton<Profile, MappingProfileExpense>();
        //
        // services.AddAutoMapper(
        //     config => { config.AddMaps(typeof(MappingProfileFinancialCategory).Assembly); },
        //     AppDomain.CurrentDomain.GetAssemblies()
        // );


        services.AddValidatorsFromAssemblyContaining<ValidatorRegister>();

        return services;
    }
}