using Application.Account.Commands;
using Domain;
using Domain.IdentityEntities;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Filters;

namespace MVC.Controllers;

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
    public async Task<IActionResult> Register(CommandRegister command)
    {
        ValidationResult validationResult = await validatorRegister.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(command);
        }

        Result result = await mediator.Send(command);
        
        if (result.IsSuccess)
        {
            return RedirectToAction("Index", "Dashboard");
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
    public async Task<IActionResult> Login(CommandLogIn command, string? returnUrl = null)
    {
        ValidationResult validationResult = await validatorLogIn.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(command);
        }
        
        Result<ResultLogIn> result = await mediator.Send(command);

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
        
        return RedirectToAction("Index", "Dashboard");
    }

    public async Task<IActionResult> Logout()
    {
        await mediator.Send(new CommandSignOut());
        return RedirectToAction("Index", "Home");
    }
}