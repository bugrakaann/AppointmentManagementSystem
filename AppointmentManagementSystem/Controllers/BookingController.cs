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
        private ICalendarService _googleCalendarService;

        public BookingController(ICustomerService customerService, IAppointmentService appointmentService,
            ICalendarService googleCalendarService)
        {
            _customerService = customerService;
            _appointmentService = appointmentService;
            _googleCalendarService = googleCalendarService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("NewAppointment")]
        public async Task<IActionResult> NewAppointment(NewAppointmentDto info)
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

                    try
                    {
                        string[] desc = [$"Email: {customer.email}", $"Tel: {customer.phoneNumber}", $"Adres: {customer.address}", $"Açýklama: {appointment.description}"];
                        var result = await _googleCalendarService.AddEventAsync(
                            $"Randevu - {customer.name} {customer.surname}",
                            string.Join("\n", desc),
                            appointment.startTime,
                            appointment.endTime
                        );
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Bir hata oluþtu");
                        return View("Index", info);
                    }

                    return View("BookingSuccess", appointment);
                }
            }

            return View("Index", info);
        }


        [HttpGet("a")]
        public async Task<IActionResult> StartWatchingCalendar()
        {
            var webhookUrl = "https://p.twitools.me/c.php";
            var calendarId = "estery.proje@gmail.com"; // Ýzlemek istediðiniz takvim ID'si

            var channel = await _googleCalendarService.WatchCalendarAsync(webhookUrl);

            // Kanal bilgilerini saklayýn (database veya baþka bir saklama çözümü)
            Console.WriteLine($"Started watching calendar. Channel ID: {channel.Id}");

            return Ok("Started watching calendar.");
        }

        [HttpGet("b")]
        public async Task<IActionResult> StopWatchingCalendar(string channelId, string resourceId)
        {
            await _googleCalendarService.StopWatchingCalendarAsync(channelId, resourceId);
            return Ok("Stopped watching calendar.");
        }

    }
}