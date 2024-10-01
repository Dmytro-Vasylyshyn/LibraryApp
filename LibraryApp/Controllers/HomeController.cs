using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Models;

namespace LibraryApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [Route("Error")]
    public IActionResult Error()
    {
        var requestId = HttpContext.TraceIdentifier; 
        return View(new ErrorViewModel { RequestId = requestId });
    }
}
