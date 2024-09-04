using System.Text.RegularExpressions;
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

    public async Task<IEnumerable<AppointmentDto>> GetByDateRange(DateOnly startDate, DateOnly endDate)
    {
        var startTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endTime = endDate.ToDateTime(TimeOnly.MaxValue);
        var list = await _appointmentRepository.GetByDateRange(startTime, endTime,
            _appointmentRepository.ValidStatuses);
        return _mapper.Map<IEnumerable<AppointmentDto>>(list);
    }

    public async Task<IEnumerable<AppointmentSlotDto>> GetSlots(DateOnly startDate, DateOnly endDate)
    {
        var list = await GetByDateRange(startDate, endDate);
        var listDto = _mapper.Map<IEnumerable<AppointmentSlotDto>>(list).ToList();
        foreach (var item in listDto)
        {
            item.Props = GetAppointmentStatus(item.Status);
        }

        return listDto;
    }

    public async Task<IEnumerable<AppointmentDetailsDto>> GetSlotsWithDetails(DateOnly startDate, DateOnly endDate)
    {
        var list = await GetByDateRange(startDate, endDate);
        var listDto = _mapper.Map<IEnumerable<AppointmentDetailsDto>>(list).ToList();
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

    public async Task ReceiveGcEventUpdates(string? channelToken)
    {
        if (string.IsNullOrEmpty(channelToken) || channelToken != _googleCalendarService.CalendarToken)
        {
            throw new ArgumentException("Geçersiz doğrulama");
        }

        var gcEvents = await _googleCalendarService.GetUpdatedEvents();
        foreach (var gcEvent in gcEvents)
        {
            try
            {
                var appointment = await _appointmentRepository.GetByGoogleEventId(gcEvent.Id);
                if (appointment != null)
                {
                    await UpdateAppointmentGcEvent(appointment, gcEvent);
                }
                else
                {
                    await AddAppointmentGcEvent(gcEvent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private async Task UpdateAppointmentGcEvent(Appointment appointment, GoogleCalendarEventDto gcEvent)
    {
        if (gcEvent.Status == "cancelled")
        {
            if (appointment.Status != AppointmentStatus.Denied)
            {
                await SetDenied(appointment);
            }

            return;
        }

        var gcStatus = GetAppointmentStatuses().Values.First(s => s.ColorId == gcEvent.ColorId);

        appointment.Status = gcStatus.Status;
        appointment.StartTime = _utilService.DateTimeOffsetToDateTime(gcEvent.StartTime);
        appointment.EndTime = _utilService.DateTimeOffsetToDateTime(gcEvent.EndTime);

        if (appointment.Customer != null)
        {
            var gcCustomer = ParseCustomer(gcEvent.Description);
            appointment.Customer.Name = gcCustomer.Name;
            appointment.Customer.Surname = gcCustomer.Surname;
            appointment.Customer.Email = gcCustomer.Email;
            appointment.Customer.PhoneNumber = gcCustomer.PhoneNumber;
        }

        await _appointmentRepository.Update(appointment);
    }

    private async Task AddAppointmentGcEvent(GoogleCalendarEventDto gcEvent)
    {
        if (gcEvent.Status == "cancelled") return;

        var startTime = _utilService.DateTimeOffsetToDateTime(gcEvent.StartTime);
        var endTime = _utilService.DateTimeOffsetToDateTime(gcEvent.EndTime);

        await CheckOverlap(startTime, endTime);

        var appointmentStatus = GetAppointmentStatuses().Values.First(s => s.ColorId == gcEvent.ColorId);

        var customer = ParseCustomer(gcEvent.Description);
        customer.Address = "-";
        var appointment = new Appointment
        {
            Status = appointmentStatus.Status,
            StartTime = startTime,
            EndTime = endTime,
            Description = "GOOGLE TAKVİMLER",
            Customer = customer,
            GoogleEventId = gcEvent.Id
        };

        await _appointmentRepository.Add(appointment);
    }

    private static Customer ParseCustomer(string description)
    {
        description = description.Replace("<br>", "\n");
        var summaryParts = description
            .Split('\n')
            .Select(s => Regex.Replace(s, "<.*?>", String.Empty).Trim())
            .ToArray();
        
        var nameField = summaryParts[0];
        var fullName = nameField.Contains(' ')
            ? nameField.Split(' ', 2)
            : [nameField, ""];

        Match email = Regex.Match(summaryParts[2], @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

        var customer = new Customer
        {
            Name = fullName[0].Trim(),
            Surname = fullName[1].Trim(),
            PhoneNumber = summaryParts[1],
            Email = email.Value,
        };

        return customer;
    }

    private async Task SetDenied(Appointment appointment)
    {
        appointment.Status = AppointmentStatus.Denied;
        await _appointmentRepository.Update(appointment);
    }

    public async Task<AppointmentDto> Deny(int id)
    {
        var appointment = await _appointmentRepository.GetById(id);
        await SetDenied(appointment);
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
        await CheckOverlap(busyingDto.StartTime, busyingDto.EndTime);

        var user = await _authService.GetLoggedUser();
        var customer = new Customer
        {
            Name = user.UserName ?? "ADMIN",
            Surname = " " ?? "ADMIN",
            PhoneNumber = "-",
            Email = user.Email ?? "",
            Address = "-"
        };

        return await AddSync(
            customer,
            AppointmentStatus.Busy,
            busyingDto.StartTime,
            busyingDto.EndTime,
            "-"
        );
    }

    public async Task<AppointmentDto> Book(BookingDto bookingDto)
    {
        CheckPastTime(bookingDto.StartTime, bookingDto.EndTime);
        await CheckOverlap(bookingDto.StartTime, bookingDto.EndTime);

        var customer = new Customer
        {
            Name = bookingDto.Name,
            Surname = bookingDto.Surname,
            PhoneNumber = bookingDto.PhoneNumber,
            Email = bookingDto.Email,
            Address = bookingDto.Address
        };

        return await AddSync(
            customer,
            AppointmentStatus.WaitingForApproval,
            bookingDto.StartTime,
            bookingDto.EndTime,
            bookingDto.Description
        );
    }

    private async Task<AppointmentDto> AddSync(Customer customer, AppointmentStatus status, DateTime startTime,
        DateTime endTime, string description)
    {
        var appointment = new Appointment
        {
            Customer = customer,
            Status = status,
            StartTime = startTime,
            EndTime = endTime,
            Description = description
        };
        appointment = await _appointmentRepository.Add(appointment);

        var gcEvent = await _googleCalendarService.AddEvent(CreateGcEvent(appointment));
        appointment.GoogleEventId = gcEvent.Id;
        await _appointmentRepository.Update(appointment);

        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> Update(SubmissionUpdateDto dto)
    {
        CheckPastTime(dto.StartTime, dto.EndTime);
        await CheckOverlap(dto.StartTime, dto.EndTime, dto.Id);

        var appointment = await _appointmentRepository.GetById(dto.Id);

        if (appointment.Customer != null)
        {
            appointment.Customer.Name = dto.Name;
            appointment.Customer.Surname = dto.Surname;
            appointment.Customer.PhoneNumber = dto.PhoneNumber;
            appointment.Customer.Email = dto.Email;
            appointment.Customer.Address = dto.Address;
        }
        appointment.StartTime = dto.StartTime;
        appointment.EndTime = dto.EndTime;
        appointment.Description = dto.Description;
        await _appointmentRepository.Update(appointment);

        if (appointment.GoogleEventId != null)
        {
            var gcEvent = CreateGcEvent(appointment);
            gcEvent.Id = appointment.GoogleEventId;
            await _googleCalendarService.UpdateEvent(gcEvent);
        }
        
        return _mapper.Map<AppointmentDto>(appointment);
    }

    private GoogleCalendarEventDto CreateGcEvent(Appointment appointment)
    {
        var customer = appointment.Customer;
        return new GoogleCalendarEventDto
        {
            Summary = "RANDEVU",
            Description = $"{customer?.Name} {customer?.Surname}\n{customer?.PhoneNumber}\n{customer?.Email}",
            ColorId = GetAppointmentStatus(appointment.Status).ColorId,
            StartTime = appointment.StartTime,
            EndTime = appointment.EndTime,
        };
    }

    private async Task CheckOverlap(DateTime starTime, DateTime endTime, int id = 0)
    {
        var overlap = await _appointmentRepository.Overlap(starTime, endTime);
        if (overlap == null) return;
        if (id == 0 || id != overlap.Id)
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

    private AppointmentStatus[] GetValidAppointmentStatusArray()
    {
        return GetAppointmentStatuses()
            .Values
            .Where(s => s.IsValid)
            .Select(s => s.Status)
            .ToArray();
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