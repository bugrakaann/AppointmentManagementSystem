using AutoMapper;
using Data_Access_Layer.Data;
using Data_Access_Layer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

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