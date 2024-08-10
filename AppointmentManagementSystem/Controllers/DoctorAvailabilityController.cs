using Microsoft.AspNetCore.Mvc;

namespace AppointmentManagementSystem.Controllers;

public class DoctorAvailabilityController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}