using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVC.Filters;

public class AllowAnonymousOnlyAttribute(string redirectController = "Home", string redirectAction = "Show")
    : AllowAnonymousAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity is { IsAuthenticated: true })
        {
            context.Result = new RedirectToActionResult(redirectAction, redirectController, new { area = "Dashboard" });
        }
    }
}