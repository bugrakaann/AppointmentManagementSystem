using AutoMapper;
using Business.Services.Abstract;
using Data_Access_Layer.Repositories.Abstract;
using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace Business.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IGoogleCalendarService _googleCalendarService;
    private readonly IAuthService _authService;
    private readonly IUtilService _utilService;

    public AppointmentService(IMapper mapper, IAppointmentRepository appointmentRepository,
        IGoogleCalendarService googleCalendarService, IAuthService authService, IUtilService utilService)
    {
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _googleCalendarService = googleCalendarService;
        _authService = authService;
        _utilService = utilService;
        Init();
    }

    private void Init()
    {
        _appointmentRepository.ValidStatuses = GetValidAppointmentStatusArray();
    }


    private AppointmentStatus[] GetValidAppointmentStatusArray()
    {
        return GetAppointmentStatuses()
            .Values
            .Where(s => s.IsValid)
            .Select(s => s.Status)
            .ToArray();
    }

    public async Task<IEnumerable<AppointmentDto>> GetByDateRange(DateOnly startDate, DateOnly endDate)
    {
        var startTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endTime = endDate.ToDateTime(TimeOnly.MaxValue);
        var list = await _appointmentRepository.GetByDateRange(startTime, endTime);
        var listDto = _mapper.Map<IEnumerable<AppointmentDto>>(list);
        return listDto;
    }

    public async Task<IEnumerable<AppointmentSlotDto>> GetSlots(DateOnly startDate, DateOnly endDate)
    {
        var list = await GetByDateRange(startDate, endDate);
        var listDto = _mapper.Map<IEnumerable<AppointmentSlotDto>>(list);
        foreach (var item in listDto)
        {
            item.Props = GetAppointmentStatus(item.Status);
        }
        return listDto;
    }

    public async Task<IEnumerable<AppointmentDetailsDto>> GetSlotsWithDetails(DateOnly startDate, DateOnly endDate)
    {
        var list = await GetByDateRange(startDate, endDate);
        var listDto = _mapper.Map<IEnumerable<AppointmentDetailsDto>>(list);
        foreach (var item in listDto)
        {
            item.Props = GetAppointmentStatus(item.Status);
            item.Url = _utilService.UrlToAction("Details", "Submissions", new { id = item.Id });
        }
        return listDto;
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
    public async Task<PagedResultDto<AppointmentDto>> GetByIdPaged(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
        var list = new List<Appointment> { appointment };
        var listDto = _mapper.Map<IEnumerable<AppointmentDto>>(list);
        return new PagedResultDto<AppointmentDto>
        {
            Items = listDto.ToList(),
            TotalItems = 1,
            PageNumber = 1,
            PageSize = 1
        };
    }

    public async Task<AppointmentDto> Deny(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
        appointment.Status = AppointmentStatus.Denied;
        await _appointmentRepository.Update(appointment);
        if (appointment.GoogleEventId != null)
        {
            await _googleCalendarService.DeleteEvent(appointment.GoogleEventId);
        }
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> Approve(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
        var appointmentStatus = GetAppointmentStatus(AppointmentStatus.Approved);
        appointment.Status = appointmentStatus.Status;
        await _appointmentRepository.Update(appointment);
        if (appointment.GoogleEventId != null)
        {
            await _googleCalendarService.UpdateEventColor(appointment.GoogleEventId, appointmentStatus.ColorId);
        }
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> Busy(BusyingDto busyingDto)
    {
        CheckPastTime(busyingDto.StartTime, busyingDto.EndTime);
        await CheckOverlap(busyingDto.StartTime, busyingDto.EndTime);

        var user = await _authService.GetLoggedUser();
        var appointmentStatus = GetAppointmentStatus(AppointmentStatus.Busy);
        var customer = new Customer
        {
            Name = user.UserName ?? "ADMIN",
            Surname = "",
            PhoneNumber = "",
            Email = user.Email ?? "",
            Address = ""
        };
        var appointment = new Appointment
        {
            Description = appointmentStatus.Title,
            Status = appointmentStatus.Status,
            StartTime = busyingDto.StartTime,
            EndTime = busyingDto.EndTime,
            Customer = customer
        };
        appointment = await _appointmentRepository.Add(appointment);

        await _googleCalendarService.AddEvent(
            $"Randevu - {appointmentStatus.Title}",
            "",
            appointment.StartTime
            , appointment.EndTime,
            appointmentStatus.ColorId
        );

        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> Book(BookingDto bookingDto)
    {
        CheckPastTime(bookingDto.StartTime, bookingDto.EndTime);
        await CheckOverlap(bookingDto.StartTime, bookingDto.EndTime);

        var appointmentStatus = GetAppointmentStatus(AppointmentStatus.WaitingForApproval);
        var customer = new Customer
        {
            Name = bookingDto.Name,
            Surname = bookingDto.Surname,
            PhoneNumber = bookingDto.PhoneNumber,
            Email = bookingDto.Email,
            Address = bookingDto.Address
        };
        var appointment = new Appointment
        {
            Description = bookingDto.Description,
            Status = appointmentStatus.Status,
            StartTime = bookingDto.StartTime,
            EndTime = bookingDto.EndTime,
            Customer = customer
        };
        appointment = await _appointmentRepository.Add(appointment);

        var title = $"Randevu - {customer.Name} {customer.Surname}";
        string[] desc =
        [
            $"Email: {customer.Email}",
            $"Tel: {customer.PhoneNumber}",
            $"Adres: {customer.Address}",
            $"Açıklama: {appointment.Description}"
        ];
        var gEvent = await _googleCalendarService.AddEvent(
            title,
            string.Join("\n", desc),
            appointment.StartTime
            , appointment.EndTime,
            appointmentStatus.ColorId
        );
        appointment.GoogleEventId = gEvent.Id;
        await _appointmentRepository.Update(appointment);

        return _mapper.Map<AppointmentDto>(appointment);
    }

    private async Task CheckOverlap(DateTime starTime, DateTime endTime)
    {
        if (await _appointmentRepository.IsOverlapping(starTime, endTime))
        {
            throw new ArgumentException("Bu tarih aralığı dolu!");
        }
    }

    private static void CheckPastTime(DateTime starTime, DateTime endTime)
    {
        if (starTime < DateTime.Now || endTime < DateTime.Now)
        {
            throw new ArgumentException("Geçmiş tarihli randevu oluşturulamaz!");
        }
    }

    public AppointmentStatusPropsDto GetAppointmentStatus(AppointmentStatus status)
    {
        var statuses = GetAppointmentStatuses();
        return statuses[status];
    }

    public IDictionary<AppointmentStatus, AppointmentStatusPropsDto> GetAppointmentStatuses()
    {
        return new Dictionary<AppointmentStatus, AppointmentStatusPropsDto>
        {
            {
                AppointmentStatus.Busy,
                new AppointmentStatusPropsDto
                {
                    Status = AppointmentStatus.Busy,
                    ColorId = "5",
                    ColorCode = "orange",
                    Title = "MÜSAİT DEĞİL",
                    IsValid = true
                }
            },
            {
                AppointmentStatus.WaitingForApproval,
                new AppointmentStatusPropsDto
                {
                    Status = AppointmentStatus.WaitingForApproval,
                    ColorId = "8",
                    ColorCode = "gray",
                    Title = "MEŞGUL",
                    IsValid = true
                }
            },
            {
                AppointmentStatus.Approved,
                new AppointmentStatusPropsDto
                {
                    Status = AppointmentStatus.Approved,
                    ColorId = "11",
                    ColorCode = "red",
                    Title = "REZERVE",
                    IsValid = true
                }
            },
            {
                AppointmentStatus.Denied,
                new AppointmentStatusPropsDto
                {
                    Status = AppointmentStatus.Denied,
                    ColorId = "9",
                    ColorCode = "blue",
                    Title = "REDDEDİLDİ",
                    IsValid = false
                }
            }
        };
    }

}