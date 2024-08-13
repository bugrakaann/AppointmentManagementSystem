
using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Services.Services;

namespace AppointmentManagementSystem.Controllers;

[Route("Appointment")]
public class AppointmentController : Controller
{
    private IAppointmentService _service;

    public AppointmentController(IAppointmentService service)
    {
        _service = service;
    }

    
    [HttpPost("SaveChanges")]
    public IActionResult SaveChanges(DateTime workStart, DateTime workEnd)
    {
        AppointmentDto dto = new AppointmentDto()
        {
            startTime = workStart,
            endTime = workEnd,
            description = "New Appointment",
            status = AppointmentStatus.Available
        };
        _service.Add(dto);

        return RedirectToAction("Availability", "AdminMenu");
    }
    
    [HttpGet("DeleteAppointment")]
    public IActionResult DeleteAppointment([FromQuery] int id)
    {
       _service.Delete(id);

        return RedirectToAction("Availability", "AdminMenu");
    }
}