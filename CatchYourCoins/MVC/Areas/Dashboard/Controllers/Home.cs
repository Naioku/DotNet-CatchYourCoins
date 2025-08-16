using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class Home : Controller
{
    public IActionResult Show() => View();
}