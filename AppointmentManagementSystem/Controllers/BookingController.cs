using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Newtonsoft.Json;
using Services.Services;

namespace AppointmentManagementSystem.Controllers
{
    public class BookingController : Controller
    {
        private IDoctorAvailabilityService _availabilityService;
        private ICustomerService _customerService;
        private IAppointmentService _appointmentService;
    
        public BookingController(IDoctorAvailabilityService availabilityService, ICustomerService customerService, IAppointmentService appointmentService)
        {
            _availabilityService = availabilityService;
            _customerService = customerService;
            _appointmentService = appointmentService;
        }
    
        public IActionResult Index(int pageIndex=0, int? activeSlotId = null)
        {
            int pageSize = 7;
            Dictionary<DateTime,IEnumerable<DoctorAvailabilityDto>> availabilityDtos = _availabilityService.GetAll();

            int totalPages = (int)Math.Ceiling((double)availabilityDtos.Count() / pageSize);
            var pagedData = availabilityDtos.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;
            ViewBag.ActiveSlotId = activeSlotId;
            return View(pagedData);
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

            _customerService.Add(customer);

            DoctorAvailabilityDto availability = _availabilityService.GetById(int.Parse(activeSlotId));

            AppointmentDto appointment = new AppointmentDto()
            {
                customerId = customer.id, //içindeki customera bak
                startTime = availability.WorkStart,
                endTime = availability.WorkEnd,
                description = "New Appointment"
            };
            
            _appointmentService.Add(appointment);
            TempData["appointment"] = JsonConvert.SerializeObject(appointment);
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