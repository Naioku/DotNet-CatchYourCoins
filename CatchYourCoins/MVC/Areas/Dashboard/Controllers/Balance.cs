using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class Balance : Controller
{
    public IActionResult Show() => View();
}