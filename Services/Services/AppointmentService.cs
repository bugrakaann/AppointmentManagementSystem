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

    public IEnumerable<AppointmentDto> GetRange(int startIndex, int count)
    {
        var list = _appointmentRepository.GetRange(startIndex, count);
        var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(list);
        SetCustomers(dtos);
        return dtos;
    }

    public IEnumerable<AppointmentDto> GetRange(AppointmentStatus category, int startIndex, int count)
    {
        var list = _appointmentRepository.GetRangeByStatus(category, startIndex, count);
        var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(list);
        SetCustomers(dtos);
        return dtos;
    }

    public bool CheckExistingSession(AppointmentDto dto)
    {
        var isAvailable = dto.status.Equals(AppointmentStatus.Available);
        return !isAvailable || (dto.customerId != null);
    }

    public int GetAppointmentNumber()
    {
        return _appointmentRepository.GetAppointmentNumber();
    }

    public int GetCountByStatus(AppointmentStatus status)
    {
        return _appointmentRepository.GetCountByStatus(status);
    }

    public IEnumerable<AppointmentDto> GetByDateRange(DateOnly startDate, DateOnly endDate)
    {
        var startTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endTime = endDate.ToDateTime(TimeOnly.MaxValue);
        var list = _appointmentRepository.GetByDateRange(startTime, endTime);
        var dtos = _mapper.Map<IEnumerable<AppointmentDto>>(list);
        return dtos;
    }

    public bool IsOverlapping(DateTime startTime, DateTime endTime)
    {
        return _appointmentRepository.IsOverlapping(startTime, endTime);
    }

}