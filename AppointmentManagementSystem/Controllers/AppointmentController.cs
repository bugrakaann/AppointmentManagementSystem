
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace AppointmentManagementSystem.Controllers;

public class AppointmentController : Controller
{
    private IAppointmentService _service;

    public AppointmentController(IAppointmentService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        var appointments = _service.GetAll();
        return View(appointments);
    }
}