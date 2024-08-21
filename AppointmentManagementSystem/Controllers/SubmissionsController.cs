using DTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.ViewModel;
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
        [HttpGet("Index/{page:int}")]
        public ActionResult Index(int page = 1, [FromQuery] int categoryId = 1)
        {
            int pageSize = 5;
            int startIndex = (page - 1) * pageSize;
            AppointmentStatus category = (AppointmentStatus)(categoryId);

            var appointments = _appointmentService.GetRange(category, startIndex, pageSize);

            var totalAppointments = _appointmentService.GetCountByStatus(category);

            var viewModel = new AppointmentViewModel
            {
                Appointments = appointments,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalAppointments / pageSize),
                CategoryId = categoryId
            };

            return View(viewModel);
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