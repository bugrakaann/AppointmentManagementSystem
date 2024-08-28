using Microsoft.AspNetCore.Mvc;
using Business.Services.Abstract;
using Models.DTOs;

namespace AppointmentManagementSystem.Controllers;

[Route("Booking")]
public class BookingController : Controller
{
    private readonly IAppointmentService _appointmentService;
    private readonly IUtilService _utilService;

    public BookingController(IAppointmentService appointmentService, IUtilService utilService)
    {
        _appointmentService = appointmentService;
        _utilService = utilService;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("Save")]
    public async Task<IActionResult> Save(BookingDto bookingDto)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", bookingDto);
        }

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