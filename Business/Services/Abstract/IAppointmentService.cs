using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace Business.Services.Abstract;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentSlotDto>> GetByDateRange(DateOnly startDate, DateOnly endDate);
    Task<PagedResultDto<AppointmentDto>> GetPaged(int pageNumber, AppointmentStatus status);
    Task<AppointmentSlotDto> Deny(int id);
    Task<AppointmentSlotDto> Approve(int id);
    Task<Appointment> Book(BookingDto bookingDto);
}