using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace AppointmentManagementSystem.Controllers
{
    [Route("Appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("GetSlots")]
        public IActionResult GetSlots([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            var slots = _appointmentService.GetByDateRange(startDate, endDate);
            return Json(slots);
        }

    }
}
