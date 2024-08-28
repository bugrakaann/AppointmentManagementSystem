using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Business.Services.Abstract;

namespace AppointmentManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly IUtilService _utilService;

    public HomeController(IUtilService utilService)
    {
        _utilService = utilService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int code = 0)
    {
        var message = _utilService.GetHttpErrorMessage(code);
        return View(new ErrorDto { Message = message });
    }

}