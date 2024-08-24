using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace AppointmentManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult HttpError(int id = 0)
    {
        string message;
        switch (id)
        {
            case 400:
                message = "Geçersiz istek!";
                break;
            case 401:
                message = "Yetkisiz eriþim!";
                break;
            case 403:
                message = "Eriþim engellendi!";
                break;
            case 404:
                message = "Sayfa bulunamadý!";
                break;
            case 500:
                message = "Sunucu hatasý!";
                break;
            case 502:
                message = "Sunucudan geçersiz yanýt!";
                break;
            case 503:
                message = "Sunucu kullanýlamýyor!";
                break;
            case 504:
                message = "Að geçidi zaman aþýmýna uðradý";
                break;
            default:
                message = "Bir hata oluþtu!";
                break;
        }
        return View(new ErrorViewModel { Message = message });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}