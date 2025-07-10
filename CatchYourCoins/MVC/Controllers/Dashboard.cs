using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

public class Dashboard : Controller
{
    public IActionResult Index() => View();
    public IActionResult AddExpense() => View();
    public IActionResult AddIncome() => View();
    public IActionResult ViewBalance() => View();
    public IActionResult Settings() => View();
}