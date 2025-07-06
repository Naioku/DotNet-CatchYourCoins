using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

public class Dashboard : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}