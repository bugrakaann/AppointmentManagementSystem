using Models.DTOs;
using Models.Enums;

namespace Business.Services.Abstract;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentSlotDto>> GetByDateRange(DateOnly startDate, DateOnly endDate);
    Task<PagedResultDto<AppointmentDto>> GetPaged(int pageNumber, AppointmentStatus status);
    Task<AppointmentDto> Deny(int id);
    Task<AppointmentDto> Approve(int id);
    Task<AppointmentDto> Book(BookingDto bookingDto);
    Task<AppointmentDto> Busy(BusyingDto busyingDto);
}