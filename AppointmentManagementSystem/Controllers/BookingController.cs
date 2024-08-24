using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Services.Services;

namespace AppointmentManagementSystem.Controllers
{
    [Route("Booking")]
    public class BookingController : Controller
    {
        private ICustomerService _customerService;
        private IAppointmentService _appointmentService;

        public BookingController(ICustomerService customerService, IAppointmentService appointmentService)
        {
            _customerService = customerService;
            _appointmentService = appointmentService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("NewAppointment")]
        public IActionResult NewAppointment(NewAppointmentDto info)
        {
            if (ModelState.IsValid)
            {
                if (info.StartTime < DateTime.Now || info.EndTime < DateTime.Now)
                {
                    ModelState.AddModelError("", "Geçersiz tarih aralýðý!");
                }
                else if (_appointmentService.IsOverlapping(info.StartTime, info.EndTime))
                {
                    ModelState.AddModelError("", "Bu tarih aralýðý dolu!");
                }
                else
                {
                    var customer = new CustomerDto
                    {
                        name = info.Name,
                        surname = info.Surname,
                        phoneNumber = info.PhoneNumber,
                        email = info.Email,
                        address = info.Address
                    };
                    _customerService.Add(customer);
                    var appointment = new AppointmentDto
                    {
                        customerId = customer.id,
                        description = info.Description,
                        status = AppointmentStatus.WaitingForApproval,
                        startTime = info.StartTime,
                        endTime = info.EndTime
                    };
                    _appointmentService.Add(appointment);
                    return View("BookingSuccess", appointment);
                }
            }

            return View("Index", info);
        }

    }
}