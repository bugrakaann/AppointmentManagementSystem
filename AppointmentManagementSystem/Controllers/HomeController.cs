using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace AppointmentManagementSystem.Controllers;

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
    
    public IActionResult Booking()
    {
        return View();
    }
    
    public IActionResult BookingSuccess()
    {
        return View();
    }
    public IActionResult Appointments()
    {
        return View();
    }
    public IActionResult ScheduleTimings()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}