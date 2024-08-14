using System.Collections;
using System.Linq.Expressions;
using AutoMapper;
using Data_Access_Layer.Repositories;
using DTOs.DTOs;
using Models.Enums;
using Models.Models;

namespace Services.Services;

public class AppointmentService : IAppointmentService
{
    private readonly ICustomerService _customerService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository appointmentRepository, ICustomerService customerService,
        IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
        _customerService = customerService;
    }

    public IEnumerable<AppointmentDto> GetAll()
    {
        var appointments = _appointmentRepository.GetAll();

        IEnumerable<AppointmentDto> dtos =
            _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        SetCustomers(dtos);

        return dtos;
    }

    public IEnumerable<AppointmentDto> GetAll(AppointmentStatus status)
    {
        var appointments = _appointmentRepository.GetAll();
        IEnumerable<AppointmentDto> dtos =
            _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        SetCustomers(dtos);

        return dtos;
    }

    private void SetCustomers(IEnumerable<AppointmentDto> dtos)
    {
        foreach (var appointment in dtos)
        {
            if (appointment.customerId == null)
                continue;
            CustomerDto foundCustomer = _customerService.GetById((int)appointment.customerId);
            appointment.SetCustomer(foundCustomer);
        }
    }

    public List<AppointmentDto> GetByDate(DateTime date)
    {
        var availabilities = _appointmentRepository.GetAll();
        var filtered = availabilities.Where(a => a.startTime.Date == date.Date);

        return _mapper.Map<List<AppointmentDto>>(filtered);
    }

    public AppointmentDto GetById(int id)
    {
        var appointment = _appointmentRepository.GetById(id);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public void Add(AppointmentDto appointment)
    {
        var _appointment = _mapper.Map<Appointment>(appointment);
        var timeSlots = GenerateTimeSlots(_appointment.startTime, _appointment.endTime);
        foreach (var slot in timeSlots)
        {
            // Query the database for overlapping appointments
            var overlappingAppointments = _appointmentRepository
                .Find(a => a.startTime < slot.Item2 && a.endTime > slot.Item1)
                .ToList();

            // If there are overlapping appointments, skip this slot
            if (overlappingAppointments.Any())
                continue;

            var newAppointment = new Appointment
            {
                startTime = slot.Item1,
                endTime = slot.Item2,
                description = _appointment.description,
                status = _appointment.status
            };

            _appointmentRepository.Add(newAppointment);
        }
    }

    private List<(DateTime, DateTime)> GenerateTimeSlots(DateTime workStart, DateTime workEnd)
    {
        var slots = new List<(DateTime, DateTime)>();

        DateTime currentStart = workStart;
        while (currentStart < workEnd)
        {
            DateTime currentEnd = currentStart.AddMinutes(30);
            if (currentEnd <= workEnd)
            {
                slots.Add((currentStart, currentEnd));
            }

            currentStart = currentEnd;
        }

        return slots;
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

    public IEnumerable<AppointmentDto> GetRange(int startIndex, int count)
    {
        var list = _appointmentRepository.GetRange(startIndex, count);
        var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(list);
        SetCustomers(dtos);
        return dtos;
    }
    public IEnumerable<AppointmentDto> GetRange(AppointmentStatus category,int startIndex, int count)
    {
        var list = _appointmentRepository.GetRangeByStatus(category, startIndex, count);
        var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(list);
        SetCustomers(dtos);
        return dtos;
    }
    //Checks if there is an existing session in the database that overlaps 
    public bool CheckExistingSession(AppointmentDto dto)
    {
        var overlappingAppointments = _appointmentRepository
            .Find(a => 
                    (a.startTime < dto.endTime && a.endTime > dto.startTime) || // General overlap
                    (a.startTime >= dto.startTime && a.endTime <= dto.endTime) || // Complete overlap
                    (a.startTime < dto.endTime && a.endTime > dto.endTime) || // Overlap at the end
                    (a.startTime < dto.startTime && a.endTime > dto.startTime) // Overlap at the beginning
            )
            .ToList();

        return overlappingAppointments.Any();
    }
    public int GetAppointmentNumber()
    {
        return _appointmentRepository.GetAppointmentNumber();
    }
}