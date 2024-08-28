using AutoMapper;
using Business.Services.Abstract;
using Data_Access_Layer.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace Business.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IGoogleCalendarService _googleCalendarService;
    private readonly IUtilService _utilService;
    private readonly IAuthService _authService;

    public AppointmentService(IMapper mapper, IAppointmentRepository appointmentRepository,
        IGoogleCalendarService googleCalendarService, IUtilService utilService,
        IAuthService authService)
    {
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _googleCalendarService = googleCalendarService;
        _utilService = utilService;
        _authService = authService;
        Init();
    }

    private void Init()
    {
        _appointmentRepository.ValidStatuses = GetValidAppointmentStatusArray();
    }


    private AppointmentStatus[] GetValidAppointmentStatusArray()
    {
        return _utilService
            .GetAppointmentStatuses()
            .Values
            .Where(s => s.IsValid)
            .Select(s => s.Status)
            .ToArray();
    }

    public async Task<IEnumerable<AppointmentSlotDto>> GetByDateRange(DateOnly startDate, DateOnly endDate)
    {
        var startTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endTime = endDate.ToDateTime(TimeOnly.MaxValue);
        var list = await _appointmentRepository.GetByDateRange(startTime, endTime);
        var listDto = _mapper.Map<IEnumerable<AppointmentSlotDto>>(list);
        return FillSlotsProps(listDto);
    }

    private IEnumerable<AppointmentSlotDto> FillSlotsProps(IEnumerable<AppointmentSlotDto> list)
    {
        foreach (var item in list)
        {
            item.Props = _utilService.GetAppointmentStatus(item.Status);
        }

        return list;
    }

    public async Task<PagedResultDto<AppointmentDto>> GetPaged(int pageNumber, AppointmentStatus status)
    {
        const int pageSize = 5;
        pageNumber = Math.Max(1, pageNumber);
        var startIndex = (pageNumber - 1) * pageSize;
        var totalCount = await _appointmentRepository.GetCountByStatus(status);
        var list = await _appointmentRepository.GetRangeByStatus(status, startIndex, pageSize);
        var listDto = _mapper.Map<IEnumerable<AppointmentDto>>(list);
        return new PagedResultDto<AppointmentDto>
        {
            Items = listDto.ToList(),
            TotalItems = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<AppointmentDto> Deny(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
        appointment.status = AppointmentStatus.Denied;
        await _appointmentRepository.Update(appointment);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> Approve(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
        appointment.status = AppointmentStatus.Approved;
        await _appointmentRepository.Update(appointment);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> Busy(BusyingDto busyingDto)
    {
        CheckPastTime(busyingDto.StartTime, busyingDto.EndTime);
        await CheckOverlap(busyingDto.StartTime, busyingDto.EndTime);

        var user = await _authService.GetLoggedUser();
        var appointmentStatus = _utilService.GetAppointmentStatus(AppointmentStatus.Busy);
        var customer = new Customer
        {
            name = user.UserName ?? "ADMIN",
            surname = "",
            phoneNumber = "",
            email = user.Email ?? "",
            address = ""
        };
        var appointment = new Appointment
        {
            description = appointmentStatus.Title,
            status = appointmentStatus.Status,
            startTime = busyingDto.StartTime,
            endTime = busyingDto.EndTime,
            customer = customer
        };
        appointment = await _appointmentRepository.Add(appointment);

        await _googleCalendarService.AddEvent(
            $"Randevu - {appointmentStatus.Title}",
            "",
            appointment.startTime
            , appointment.endTime,
            appointmentStatus.ColorId
        );

        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> Book(BookingDto bookingDto)
    {
        CheckPastTime(bookingDto.StartTime, bookingDto.EndTime);
        await CheckOverlap(bookingDto.StartTime, bookingDto.EndTime);

        var appointmentStatus = _utilService.GetAppointmentStatus(AppointmentStatus.Busy);
        var customer = new Customer
        {
            name = bookingDto.Name,
            surname = bookingDto.Surname,
            phoneNumber = bookingDto.PhoneNumber,
            email = bookingDto.Email,
            address = bookingDto.Address
        };
        var appointment = new Appointment
        {
            description = bookingDto.Description,
            status = appointmentStatus.Status,
            startTime = bookingDto.StartTime,
            endTime = bookingDto.EndTime,
            customer = customer
        };
        appointment = await _appointmentRepository.Add(appointment);

        var title = $"Randevu - {customer.name} {customer.surname}";
        string[] desc =
        [
            $"Email: {customer.email}",
            $"Tel: {customer.phoneNumber}",
            $"Adres: {customer.address}",
            $"Açýklama: {appointment.description}"
        ];
        await _googleCalendarService.AddEvent(
            title,
            string.Join("\n", desc),
            appointment.startTime
            , appointment.endTime,
            appointmentStatus.ColorId
        );

        return _mapper.Map<AppointmentDto>(appointment);
    }

    private async Task CheckOverlap(DateTime starTime, DateTime endTime)
    {
        if (await _appointmentRepository.IsOverlapping(starTime, endTime))
        {
            throw new ArgumentException("Bu tarih aralýðý dolu!");
        }
    }

    private static void CheckPastTime(DateTime starTime, DateTime endTime)
    {
        if (starTime < DateTime.Now || endTime < DateTime.Now)
        {
            throw new ArgumentException("Geçmiþ tarihli randevu oluþturulamaz!");
        }
    }
}