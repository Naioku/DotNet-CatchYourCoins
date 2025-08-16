using Application.Account.Commands;
using Domain;
using Domain.IdentityEntities;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using MVC.Filters;

namespace MVC.Areas.Public.Controllers;

[Area("Public")]
[AllowAnonymous]
public class Account(
    IMediator mediator,
    ValidatorRegister validatorRegister,
    ValidatorLogIn validatorLogIn) : Controller
{
    [AllowAnonymousOnly]
    public IActionResult Register() => View();

    [AllowAnonymousOnly]
    [HttpPost]
    public async Task<IActionResult> Register(CommandRegister command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validatorRegister.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(command);
        }

        Result result = await mediator.Send(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            return RedirectToAction("Show", "Home", new { area = "Dashboard" });
        }

        foreach (KeyValuePair<string, string> error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Value);
        }

        return View(command);
    }

    [AllowAnonymousOnly]
    public IActionResult Login() => View();

    [AllowAnonymousOnly]
    [HttpPost]
    public async Task<IActionResult> Login(CommandLogIn command, CancellationToken cancellationToken, string? returnUrl = null)
    {
        ValidationResult validationResult = await validatorLogIn.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(command);
        }
        
        Result<ResultLogIn> result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            foreach (KeyValuePair<string, string> error in result.Errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return View(command);
        }

        if (result.Value.RequiresTwoFactor)
        {
            // Todo: Two-factor auth.
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }
        
        return RedirectToAction("Show", "Home", new { area = "Dashboard" });
    }

    public async Task<IActionResult> Logout()
    {
        await mediator.Send(new CommandSignOut());
        return RedirectToAction("Index", "Home", new { area = "Public" });
    }
}