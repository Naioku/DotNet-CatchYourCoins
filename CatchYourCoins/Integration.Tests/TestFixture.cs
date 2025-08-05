using Application.Extensions;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddSingleton<TestServiceCurrentUser>();
        services.AddScoped<IServiceCurrentUser>(sp => sp.GetRequiredService<TestServiceCurrentUser>());

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Main"))
        );
        
        ServiceProvider = services.BuildServiceProvider();

        using (IServiceScope scope = ServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            using (IServiceScope scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureDeleted();
                _disposed = true;
            }
        }

        GC.SuppressFinalize(this);
    }
}