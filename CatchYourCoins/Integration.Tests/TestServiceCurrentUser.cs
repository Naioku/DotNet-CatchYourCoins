using Domain.Dashboard.Entities;
using Domain.IdentityEntities;
using Domain.Interfaces.Services;

namespace Integration;

public class TestServiceCurrentUser() : IServiceCurrentUser
{
    private AppUser? _appUser;

    public void SetAppUser(AppUser appUser) => _appUser = appUser;

    public CurrentUser User
    {
        get
        {
            if (_appUser == null)
            {
                throw new Exception("App user is null");
            }
            
            return new CurrentUser
            {
                Id = _appUser.Id,
                Name = _appUser.UserName!,
                Email = _appUser.Email!,
                IsAuthenticated = true,
            };
        }
    }
}