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
                message = "Ge�ersiz istek!";
                break;
            case 401:
                message = "Yetkisiz eri�im!";
                break;
            case 403:
                message = "Eri�im engellendi!";
                break;
            case 404:
                message = "Sayfa bulunamad�!";
                break;
            case 500:
                message = "Sunucu hatas�!";
                break;
            case 502:
                message = "Sunucudan ge�ersiz yan�t!";
                break;
            case 503:
                message = "Sunucu kullan�lam�yor!";
                break;
            case 504:
                message = "A� ge�idi zaman a��m�na u�rad�";
                break;
            default:
                message = "Bir hata olu�tu!";
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