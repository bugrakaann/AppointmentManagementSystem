using DTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Services.Services;

namespace AppointmentManagementSystem.Controllers
{
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
        [HttpGet("Index/{index:int}")]
        public ActionResult Index(int index = 1)
        {
            //pagination eklenirse buranın düzenlenmesi lazım
            int pageSize = 7;
            AppointmentStatus category = (AppointmentStatus)index;
            var pendingappointments = _appointmentService.GetRange(category, 0 * pageSize, 7);

            ViewBag.Category = (int)category;
            return View(pendingappointments);
        }

        [HttpGet("Reject")]
        public IActionResult Reject([FromQuery] int id)
        {
            AppointmentDto appointment = _appointmentService.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            appointment.SetStatus(AppointmentStatus.Available);
            appointment.FlushCustomer();
            _appointmentService.Update(appointment);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Accept")]
        public IActionResult Accept([FromQuery] int id)
        {
            AppointmentDto appointment = _appointmentService.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            appointment.SetStatus(AppointmentStatus.Approved);
            _appointmentService.Update(appointment);
            return RedirectToAction(nameof(Index));
        }
    }
}