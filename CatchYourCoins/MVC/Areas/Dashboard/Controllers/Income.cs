using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class Income : Controller
{
    public IActionResult Create() => View();
}