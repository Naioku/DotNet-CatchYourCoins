using Domain.IdentityEntities;
using Infrastructure.Persistence;
using Integration.Factories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public class TestBase(TestFixture fixture) : IClassFixture<TestFixture>, IAsyncLifetime
{
    private readonly UserManager<AppUser> _userManager = fixture.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    protected readonly IMediator mediator = fixture.ServiceProvider.GetRequiredService<IMediator>();
    protected readonly AppDbContext dbContext = fixture.ServiceProvider.GetRequiredService<AppDbContext>();
    protected AppUser user1 = null!;
    protected AppUser user2 = null!;

    public virtual async Task InitializeAsync() => await RegisterUsers();

    private async Task RegisterUsers()
    {
        user1 = await RegisterUser(TestFactoryUsers.DefaultUser1());
        user2 = await RegisterUser(TestFactoryUsers.DefaultUser2());
        fixture.ServiceProvider.GetRequiredService<TestServiceCurrentUser>().SetAppUser(user1);
    }

    private async Task<AppUser> RegisterUser(User user)
    {
        var appUser = new AppUser
        {
            Email = user.Email,
            UserName = user.Name
        };
        
        IdentityResult result = await _userManager.CreateAsync(
            appUser,
            user.Password
        );
        
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create test user 1: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        
        return appUser;
    }

    public virtual async Task DisposeAsync()
    {
        dbContext.RemoveRange(dbContext.Users);
        await dbContext.SaveChangesAsync();
    }
}