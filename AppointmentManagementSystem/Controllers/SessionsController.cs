using Microsoft.AspNetCore.Mvc;
using Business.Services.Abstract;
using Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace AppointmentManagementSystem.Controllers;


[Authorize(Roles = "Admin")]
[Route("Sessions")]
public class SessionsController : Controller
{
    private readonly IAppointmentService _appointmentService;

    public SessionsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("Busying")]
    public async Task<IActionResult> Busying(BusyingDto busyingDto)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", busyingDto);
        }

        try
        {
            await _appointmentService.Busy(busyingDto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Index", busyingDto);
        }

        return View("Index");
    }
}