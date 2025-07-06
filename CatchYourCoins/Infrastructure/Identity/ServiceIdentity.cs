using Domain;
using Domain.IdentityEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ServiceIdentity(UserManager<AppUser> managerUser, SignInManager<AppUser> managerSignIn) : IServiceIdentity
{
    public async Task<Result> RegisterUserAsync(string email, string userName, string password)
    {
        AppUser user = new()
        {
            Email = email,
            UserName = userName,
        };

        IdentityResult result = await managerUser.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await managerSignIn.SignInAsync(user, false);
            return Result.Success();
        }

        Dictionary<string, string> errors = new();
        foreach (IdentityError error in result.Errors)
        {
            errors.Add(error.Code, error.Description);
        }
        
        return Result.Failure(errors);
    }

    public async Task<Result<ResultLogIn>> LogInAsync(string email, string password)
    {
        AppUser? user = await managerUser.FindByEmailAsync(email);
        if (user == null)
        {
            return Result<ResultLogIn>.Failure(new Dictionary<string, string> { { "Email", "Email does not exist." } });
        }
        
        SignInResult result = await managerSignIn.PasswordSignInAsync(user, password, false, false);

        if (result.IsLockedOut)
        {
            return Result<ResultLogIn>.Failure(new Dictionary<string, string> { { "Account", "Account is locked." } });
        }

        if (result.IsNotAllowed)
        {
            return Result<ResultLogIn>.Failure(new Dictionary<string, string> { { "Account", "Account is not allowed to sign in." } });
        }
        
        if (result.RequiresTwoFactor)
        {
            return Result<ResultLogIn>.SetValue(ResultLogIn.TwoFactorRequired());
        }
        
        return result.Succeeded
            ? Result<ResultLogIn>.SetValue(ResultLogIn.Success())
            : Result<ResultLogIn>.Failure(new Dictionary<string, string> { { "Password", "Invalid password." } });
    }

    public async Task SignOut() => await managerSignIn.SignOutAsync();
}