using AppointmentManagementSystem.Models;
using AppointmentManagementSystem.Repositories;
using AppointmentManagementSystem.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentManagementSystem.Controllers;

public class AppointmentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public AppointmentController(ApplicationDbContext context, IMapper _mapper)
    {
        this._context = context;
        this._mapper = _mapper;
    }
    
    public IActionResult Index()
    {
        AppointmentRepository repo = new AppointmentRepository(_context);
        AppointmentService service = new AppointmentService(repo,_mapper);
        var appointments = service.GetAll();
        return View(appointments);
    }
}