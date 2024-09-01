using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.DTOs;
using Business.Services.Abstract;

namespace AppointmentManagementSystem.Controllers;

[Authorize(Roles = "Admin")]
[Route("Submissions")]
public class SubmissionsController : Controller
{
    private readonly IAppointmentService _appointmentService;

    public SubmissionsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("")]
    [HttpGet("Index/{page:int}")]
    public async Task<IActionResult> Index(
        [FromRoute] int page = 1,
        [FromQuery] AppointmentStatus status = AppointmentStatus.WaitingForApproval
    )
    {
        var appointments = await _appointmentService.GetPaged(page, status);
        var view = new SubmissionsDto
        {
            PagedResult = appointments,
            AppointmentStatus = status
        };
        return View(view);
    }


    [HttpGet("Deny/{Id:int}")]
    public async Task<IActionResult> Deny([FromRoute] int id)
    {
        await _appointmentService.Deny(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Approve/{Id:int}")]
    public async Task<IActionResult> Approve([FromRoute] int id)
    {
        await _appointmentService.Approve(id);
        return RedirectToAction(nameof(Index));
    }
}
