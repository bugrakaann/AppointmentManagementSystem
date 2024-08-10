using AutoMapper;
using Data_Access_Layer.Repositories;
using DTOs.DTOs;
using Models.Models;

namespace Services.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;
    
    public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
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