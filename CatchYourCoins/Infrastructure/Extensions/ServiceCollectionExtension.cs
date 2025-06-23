using Domain.IdentityEntities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(
            option => option
                .UseSqlServer(configuration.GetConnectionString("Main")));

        // services
        //     .AddIdentityCore<AppUser>()
        //     .AddRoles<AppRole>()
        //     .AddEntityFrameworkStores<AppDbContext>()
        //     .AddDefaultTokenProviders()
        //     .AddUserStore<UserStore<AppUser, AppRole, AppDbContext, Guid>>()
        //     .AddRoleStore<RoleStore<AppRole, AppDbContext, Guid>>();
        
        // services.AddScoped<>();
        
        return services;
    }
}