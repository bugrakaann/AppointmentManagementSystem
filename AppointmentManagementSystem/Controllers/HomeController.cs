using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using Business.Services.Abstract;

namespace AppointmentManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly IHomeService _homeService;

    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int id = 0)
    {
        var message = _homeService.GetHttpErrorMessage(id);
        return View(new ErrorViewModel { Message = message });
    }

}