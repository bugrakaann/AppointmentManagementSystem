using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.Models;
using Newtonsoft.Json;
using Services.Services;

namespace AppointmentManagementSystem.Controllers
{
    public class BookingController : Controller
    {
        private ICustomerService _customerService;
        private IAppointmentService _appointmentService;
    
        public BookingController(ICustomerService customerService, IAppointmentService appointmentService)
        {
            _customerService = customerService;
            _appointmentService = appointmentService;
        }
    
        public IActionResult Index(int pageIndex=0, int? activeSlotId = null)
        {
            int pageSize = 7;
            IEnumerable<AppointmentDto> availabilityDtos = _appointmentService.GetRange(pageIndex*pageSize, pageSize);
            

            ViewBag.PageIndex = pageIndex;
            ViewBag.ActiveSlotId = activeSlotId;
            return View(availabilityDtos);
        }

        [HttpPost("SaveChanges")]
        public IActionResult SaveChanges(string Name, string Surname, string PhoneNumber, string Email, string Address, string activeSlotId)
        {
            CustomerDto customer = new CustomerDto()
            {
                name = Name,
                surname = Surname,
                phoneNumber = PhoneNumber,
                email = Email,
                address = Address
            };
            AppointmentDto availability = _appointmentService.GetById(int.Parse(activeSlotId));
            if (_appointmentService.CheckExistingSession(availability))
            {
                TempData["Notification"] = "This session is already booked.";
                return RedirectToAction("Index", "Booking");
            }
                
            
            _customerService.Add(customer);
            availability.SetCustomerId(customer);
            availability.SetStatus(AppointmentStatus.WaitingForApproval);

            _appointmentService.Update(availability);
            TempData["appointment"] = JsonConvert.SerializeObject(availability);
            return RedirectToAction("BookingSuccess", "Booking");
        }
        public IActionResult BookingSuccess()
        {
            if (TempData["Appointment"] != null)
            {
                var appointment = JsonConvert.DeserializeObject<AppointmentDto>(TempData["Appointment"].ToString());
                return View(appointment);
            }
            return View();
        }
    }
}