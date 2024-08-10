using DTOs.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Services;

namespace AppointmentManagementSystem.Controllers;

[Route("DoctorAvailability")]
public class DoctorAvailabilityController : Controller
{

    private IDoctorAvailabilityService _availabilityService;
    
    public DoctorAvailabilityController(IDoctorAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }
    
    // GET
    public IActionResult Index(int pageIndex=0)
    {
        int pageSize = 7;
        Dictionary<DateTime,IEnumerable<DoctorAvailabilityDto>> availabilityDtos = _availabilityService.GetAll();

        int totalPages = (int)Math.Ceiling((double)availabilityDtos.Count() / pageSize);
        var pagedData = availabilityDtos.Skip(pageIndex * pageSize).Take(pageSize).ToList();

        ViewBag.PageIndex = pageIndex;
        ViewBag.TotalPages = totalPages;
        return View(pagedData);
    }
    
    [HttpPost("SaveChanges")]
    public IActionResult SaveChanges(DateTime WorkStart, DateTime WorkEnd)
    {
        DoctorAvailabilityDto doctorAvailability = new DoctorAvailabilityDto()
        {
            WorkStart = WorkStart,
            WorkEnd = WorkEnd
        };
        _availabilityService.Add(doctorAvailability);

        return RedirectToAction("Index", "DoctorAvailability");
    }

    [HttpGet("DeleteSession")]
    public IActionResult DeleteSession([FromQuery] int id)
    {
       _availabilityService.Delete(id);

        return RedirectToAction("Index", "DoctorAvailability");
    }
    
}