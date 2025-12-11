using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FlowerShop.Models;

namespace FlowerShop.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // The simplified action just returns the view without requiring a complex Model.
        return View();
    }
}
