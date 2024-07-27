using AutoMapper;
using Data_Access_Layer.Data;
using Data_Access_Layer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Services.Services;

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