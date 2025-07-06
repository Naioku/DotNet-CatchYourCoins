using Application.Account.Commands;
using Domain;
using Domain.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Filters;
using MVC.Models.Account;

namespace MVC.Controllers;

[AllowAnonymous]
public class Account(
    HandlerRegister handlerRegister,
    HandlerSignOut handlerSignOut,
    HandlerSignIn handlerSignIn) : Controller
{
    [AllowAnonymousOnly]
    public IActionResult Register() => View();

    [AllowAnonymousOnly]
    [HttpPost]
    public async Task<IActionResult> Register(Register model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Result result = await handlerRegister.Handle(new CommandRegister
        {
            Email = model.Email,
            UserName = model.Name,
            Password = model.Password
        });
        
        if (result.IsSuccess)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        foreach (KeyValuePair<string, string> error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Value);
        }

        return View(model);
    }

    [AllowAnonymousOnly]
    public IActionResult Login() => View();

    [AllowAnonymousOnly]
    [HttpPost]
    public async Task<IActionResult> Login(Login model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        Result<ResultSignIn> result = await handlerSignIn.Handle(new CommandSignIn
        {
            Email = model.Email,
            Password = model.Password
        });

        if (!result.IsSuccess)
        {
            foreach (KeyValuePair<string, string> error in result.Errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return View(model);
        }

        if (result.Value.RequiresTwoFactor)
        {
            // Todo: Two-factor auth.
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }
        
        return RedirectToAction("Index", "Dashboard");
    }

    public async Task<IActionResult> Logout()
    {
        await handlerSignOut.Handle();
        return RedirectToAction("Index", "Home");
    }
}