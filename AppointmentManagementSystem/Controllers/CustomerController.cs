using AppointmentManagementSystem.Models;
using AppointmentManagementSystem.Repositories;
using AppointmentManagementSystem.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentManagementSystem.Controllers;

public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CustomerController(ApplicationDbContext context, IMapper _mapper)
    {
        this._context = context;
        this._mapper = _mapper;
    }
    public IActionResult Index()
    {
        Repository<Customer> repo = new Repository<Customer>(_context);
        CustomerService service = new CustomerService(repo,_mapper);
        var customers = service.GetAll();
        return View(customers);
    }
}