using Domain.IdentityEntities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ExtensionServiceCollection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Main"))
        );

        services
            .AddIdentity<AppUser, AppRole>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IServiceIdentity, ServiceIdentity>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IServiceCurrentUser, ServiceCurrentUser>();
        services.AddScoped<IRepositoryExpenseCategory, RepositoryExpenseCategory>();
        services.AddScoped<IRepositoryExpensePaymentMethod, RepositoryExpensePaymentMethod>();
        services.AddScoped<IRepositoryExpense, RepositoryExpense>();
        services.AddScoped<IRepositoryIncomeCategory, RepositoryIncomeCategory>();
        services.AddScoped<IRepositoryIncome, RepositoryIncome>();

        return services;
    }
}