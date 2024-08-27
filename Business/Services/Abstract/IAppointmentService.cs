using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace Business.Services.Abstract;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetByDateRange(DateOnly startDate, DateOnly endDate);
    Task<PagedResultDto<AppointmentDto>> GetPaged(int pageNumber, AppointmentStatus status);
    Task<AppointmentDto> Deny(int id);
    Task<AppointmentDto> Approve(int id);
    Task Book(BookingDto bookingDto);
}