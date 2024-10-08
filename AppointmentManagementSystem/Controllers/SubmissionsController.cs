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

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id = 1)
    {
        var appointments = await _appointmentService.GetByIdPaged(id);
        var view = new SubmissionsDto
        {
            PagedResult = appointments,
            AppointmentStatus = appointments.Items.First().Status
        };
        return View("Index", view);
    }


    [HttpGet("Deny/{Id:int}")]
    public async Task<IActionResult> Deny([FromRoute] int id)
    {
        await _appointmentService.Deny(id);
        return RedirectToAction("Index");
    }

    [HttpGet("Approve/{Id:int}")]
    public async Task<IActionResult> Approve([FromRoute] int id)
    {
        await _appointmentService.Approve(id);
        return RedirectToAction("Index");
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(SubmissionUpdateDto submissionUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return View("Error", new ErrorDto { Message = "Geçersiz istek!" });
        }

        try
        {
            await _appointmentService.Update(submissionUpdateDto);
            return RedirectToAction("Details", new { submissionUpdateDto.Id });
        }
        catch (ArgumentException ex)
        {
            return View("Error", new ErrorDto { Message = ex.Message });
        }
    }
}