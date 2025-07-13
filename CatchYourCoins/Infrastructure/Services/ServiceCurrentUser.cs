using System.Security.Claims;
using Domain.Dashboard.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class ServiceCurrentUser(IHttpContextAccessor httpContextAccessor) : IServiceCurrentUser
{
    private CurrentUser? _cachedUser;

    public CurrentUser User
    {
        get
        {
            if (_cachedUser != null)
            {
                return _cachedUser;
            }

            HttpContext? httpContext = httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return CurrentUser.Anonymous;
            }

            string? userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return CurrentUser.Anonymous;
            }
            
            string? userName = httpContext.User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("User name is null");
            }
            
            string? email = httpContext.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null");
            }

            _cachedUser = new CurrentUser
            {
                Id = Guid.Parse(userId),
                Name = userName,
                Email = email,
                IsAuthenticated = true,
            };

            return _cachedUser;
        }
    }
}