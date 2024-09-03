using Microsoft.AspNetCore.Mvc;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace AppointmentManagementSystem.Controllers;

[Route("Appointments")]
public class AppointmentsController : Controller
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("GetSlots")]
    public async Task<IActionResult> GetSlots([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        var slots = await _appointmentService.GetSlots(startDate, endDate);
        return Json(slots);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetSlotsWithDetails")]
    public async Task<IActionResult> GetSlotsWithDetails([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        var slots = await _appointmentService.GetSlotsWithDetails(startDate, endDate);
        return Json(slots);
    }

    [HttpPost("ReceiveEventUpdates")]
    public async Task<IActionResult> ReceiveEventUpdates()
    {
        try
        {
            Request.Headers.TryGetValue("X-Goog-Channel-Token", out var token);
            await _appointmentService.ReceiveGcEventUpdates(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
    
}