
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace AppointmentManagementSystem.Controllers;

public class CustomerController : Controller
{
     ICustomerService _customerService;
     
     public CustomerController(ICustomerService customerService)
     {
         _customerService = customerService;
     }

     public IActionResult Index()
    {
        var customers = _customerService.GetAll();
        return View(customers);
    }
    
}