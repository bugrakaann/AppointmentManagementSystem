using Microsoft.AspNetCore.Mvc;
using Business.Services.Abstract;
using Models.DTOs;

namespace AppointmentManagementSystem.Controllers;

[Route("Booking")]
public class BookingController : Controller
{
    private readonly IAppointmentService _appointmentService;

    public BookingController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("Save")]
    public async Task<IActionResult> Save(BookingDto bookingDto)
    {
        try
        {
            await _appointmentService.Book(bookingDto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Index", bookingDto);
        }

        return View("BookingSuccess", bookingDto);
    }
}