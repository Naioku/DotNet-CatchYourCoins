using Domain.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Account;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MVC.Controllers;

[AllowAnonymous]
public class Account(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : Controller
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(Register model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await _userManager.FindByEmailAsync(model.Email) != null)
        {
            ModelState.AddModelError(string.Empty, "Email already exists");
            return View(model);
        }
        
        AppUser user = new()
        {
            Email = model.Email,
            UserName = model.Name,
        };

        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(Login model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        AppUser? user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("Email", "Email does not exist");
            return View(model);
        }

        if (await _signInManager.PasswordSignInAsync(user, model.Password, false, false) != SignInResult.Success)
        {
            ModelState.AddModelError("Password", "Invalid password");
            return View(model);
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }
        
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}