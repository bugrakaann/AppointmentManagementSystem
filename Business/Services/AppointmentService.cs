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

    public AppointmentService(IMapper mapper, IAppointmentRepository appointmentRepository,
        IGoogleCalendarService googleCalendarService)
    {
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _googleCalendarService = googleCalendarService;
    }

    public async Task<IEnumerable<AppointmentDto>> GetByDateRange(DateOnly startDate, DateOnly endDate)
    {
        var startTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endTime = endDate.ToDateTime(TimeOnly.MaxValue);
        var list = await _appointmentRepository.GetByDateRange(startTime, endTime);
        var listDto = _mapper.Map<IEnumerable<AppointmentDto>>(list);
        return listDto;
    }

    public async Task<PagedResultDto<AppointmentDto>> GetPaged(int pageNumber, AppointmentStatus status)
    {
        const int pageSize = 1;
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
        appointment.status = AppointmentStatus.Available;
        appointment.FlushCustomer();
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

    public async Task Book(BookingDto bookingDto)
    {
        CheckPastTime(bookingDto);
        await CheckOverlap(bookingDto);

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
            customerId = customer.id,
            description = bookingDto.Description,
            status = AppointmentStatus.WaitingForApproval,
            startTime = bookingDto.StartTime,
            endTime = bookingDto.EndTime,
            customer = customer
        };
        appointment = await _appointmentRepository.Add(appointment);

        await AddToGoogleCalendar(customer, appointment);
    }

    private async Task AddToGoogleCalendar(Customer customer, Appointment appointment)
    {
        string title = $"Randevu - {customer.name} {customer.surname}";
        string[] desc =
        [
            $"Email: {customer.email}",
            $"Tel: {customer.phoneNumber}",
            $"Adres: {customer.address}",
            $"Açýklama: {appointment.description}"
        ];
        await _googleCalendarService.AddEventAsync(
            title,
            string.Join("\n", desc),
            appointment.startTime
            , appointment.endTime
        );
    }

    private async Task CheckOverlap(BookingDto bookingDto)
    {
        if (await _appointmentRepository.IsOverlapping(bookingDto.StartTime, bookingDto.EndTime))
        {
            throw new ArgumentException("Bu tarih aralýðý dolu!");
        }
    }

    private static void CheckPastTime(BookingDto bookingDto)
    {
        if (bookingDto.StartTime < DateTime.Now || bookingDto.EndTime < DateTime.Now)
        {
            throw new ArgumentException("Geçmiþ tarihli randevu oluþturulamaz!");
        }
    }


}