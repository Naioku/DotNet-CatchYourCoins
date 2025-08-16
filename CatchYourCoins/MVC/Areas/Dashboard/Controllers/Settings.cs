using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class Settings : Controller
{
    public IActionResult Show() => View();
}