using Application.Account.Commands;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration;

[UsedImplicitly]
public class TestFixture : IDisposable
{
    private bool _disposed;
    
    public IServiceProvider ServiceProvider { get; }

    public TestFixture()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Tests.json", optional: false)
            .Build();

        
        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging(builder => 
        {
            builder.AddConsole();
            builder.AddDebug();
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Main"))
        );
        services.AddScoped<IRepositoryCategory, RepositoryCategory>();
        services.AddScoped<IRepositoryPaymentMethod, RepositoryPaymentMethod>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IServiceCurrentUser, TestServiceCurrentUser>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CommandRegister>();
        });

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureDeleted();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}