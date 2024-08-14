using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Services.Services;

namespace AppointmentManagementSystem.Controllers
{
    [Route("AdminMenu")]
    public class AdminMenuController : Controller
    {
        private readonly IAppointmentService _appointmentService;
         // private readonly IEnumerable<AppointmentDto> test;

        public AdminMenuController(IAppointmentService appointmentService)
        {
               // var dates = test.Select(w => w.startTime).Distinct().ToList();
               // foreach (var date in dates)
               // {
               //     var dataOfDate = test.Where(w => w.startTime == date);
               // }

            _appointmentService = appointmentService;
        }

        // GET: AdminMenu
        public ActionResult Index(int index = 1)
        {
            //pagination eklenirse buranın düzenlenmesi lazım
            int pageSize = 7;
            AppointmentStatus category = (AppointmentStatus)index;
            var pendingappointments = _appointmentService.GetRange(category, 0 * pageSize, 7);

            ViewBag.Category = (int)category;
            return View(pendingappointments);
        }

        [Route("Availability")]
        public IActionResult Availability(int pageIndex = 0)
        {
            int pageSize = 7;
            int totalAppointments = _appointmentService.GetAppointmentNumber();
            IEnumerable<AppointmentDto> availabilityDtos = _appointmentService.GetRange(pageIndex * pageSize,7);
            
            ViewBag.PageIndex = pageIndex;
            ViewBag.HasMorePages = (pageIndex + 1) * pageSize < totalAppointments; // Check if there are more pages
            return View(availabilityDtos);
        }

        [HttpPost("SaveChanges")]
        public IActionResult SaveChanges(DateTime workStart, DateTime workEnd)
        {
            AppointmentDto appointment = new AppointmentDto()
            {
                startTime = workStart,
                endTime = workEnd
            };
            _appointmentService.Add(appointment);

            return RedirectToAction("Availability", "AdminMenu");
        }

        [HttpGet("DeleteSession")]
        public IActionResult DeleteSession([FromQuery] int id)
        {
            _appointmentService.Delete(id);

            return RedirectToAction("Availability", "AdminMenu");
        }

        [HttpGet("RejectAppointmentRequest")]
        public IActionResult RejectAppointmentRequest([FromQuery] int id)
        {
            AppointmentDto appointmentDto = _appointmentService.GetById(id);
            appointmentDto.SetStatus(AppointmentStatus.Available);
            appointmentDto.FlushCustomer();
            _appointmentService.Update(appointmentDto);

            return RedirectToAction("Index", "AdminMenu");
        }

        [HttpGet("AcceptAppointmentRequest")]
        public IActionResult AcceptAppointmentRequest([FromQuery] int id)
        {
            AppointmentDto appointmentDto = _appointmentService.GetById(id);
            appointmentDto.SetStatus(AppointmentStatus.Approved);
            _appointmentService.Update(appointmentDto);

            return RedirectToAction("Index", "AdminMenu");
        }
    }
}