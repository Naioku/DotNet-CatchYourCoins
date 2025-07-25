using Domain.IdentityEntities;

namespace Domain.Interfaces.Services;

public interface IServiceIdentity
{
    Task<Result> RegisterUserAsync(string email, string userName, string password);
    Task<Result<ResultLogIn>> LogInAsync(string email, string password);
    Task SignOut();
}