using Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Account;

namespace MVC.Controllers;

public class Account(UserManager<AppUser> userManager) : Controller
{
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await userManager.FindByEmailAsync(model.Email) != null)
        {
            ModelState.AddModelError(string.Empty, "Email already exists");
            return View(model);
        }
        
        AppUser user = new()
        {
            Email = model.Email,
            UserName = model.Name,
        };

        IdentityResult result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }
}