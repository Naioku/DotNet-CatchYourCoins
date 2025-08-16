using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class Expense : Controller
{
    public IActionResult Create() => View();
}