﻿using Microsoft.AspNetCore.Mvc;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> GetSlots([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            var slots = await _appointmentService.GetByDateRange(startDate, endDate);
            return Json(slots);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetSlotsWithDetails")]
        public async Task<IActionResult> GetSlotsWithDetails([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            var slots = await _appointmentService.GetByDateRange(startDate, endDate);
            return Json(slots);
        }

    }
}
