using System.Collections;
using AppointmentManagementSystem.DTOs;
using AppointmentManagementSystem.Models;
using AppointmentManagementSystem.Repositories;
using AutoMapper;

namespace AppointmentManagementSystem.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly IMapper _mapper;

    public AppointmentService(IRepository<Appointment> repo, IMapper mapper)
    {
        _appointmentRepository = repo;
        _mapper = mapper;
    }

    public IEnumerable<AppointmentDto> GetAll()
    {
        var appointments = _appointmentRepository.GetAll();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public AppointmentDto GetById(int id)
    {
        var appointment = _appointmentRepository.GetById(id);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public void Add(AppointmentDto appointment)
    {
        var _appointment = _mapper.Map<Appointment>(appointment);
        _appointmentRepository.Add(_appointment);
    }

    public void Update(AppointmentDto appointment)
    {
        var _appointment = _mapper.Map<Appointment>(appointment);
        _appointmentRepository.Update(_appointment);
    }

    public void Delete(int id)
    {
        _appointmentRepository.Delete(id);
    }

    public void Delete(AppointmentDto appointment)
    {
        var _appointment = _mapper.Map<Appointment>(appointment);
        _appointmentRepository.Delete(_appointment);
    }
}