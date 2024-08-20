using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Services.Services;

namespace AppointmentManagementSystem.Controllers
{
    [Route("Sessions")]
    public class SessionsController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public SessionsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("")]
        [HttpGet("Index/{pageIndex:int}")]
        public IActionResult Index(int pageIndex = 0)
        {
            int pageSize = 7;
            int totalAppointments = _appointmentService.GetAppointmentNumber();
            IEnumerable<AppointmentDto> availabilityDtos = _appointmentService.GetRange(pageIndex * pageSize, 7);

            ViewBag.PageIndex = pageIndex;
            ViewBag.HasMorePages = (pageIndex + 1) * pageSize < totalAppointments; // Check if there are more pages
            return View(availabilityDtos);
        }

        [HttpPost("Create")]
        public IActionResult Create(DateTime workStart, DateTime workEnd)
        {
            AppointmentDto dto = new AppointmentDto()
            {
                startTime = workStart,
                endTime = workEnd,
                description = "Yeni Randevu",
                status = AppointmentStatus.Available
            };
            _appointmentService.Add(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete")]
        public IActionResult Delete([FromQuery] int id)
        {
            _appointmentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
