using Models.DTOs;
using Models.Enums;

namespace Business.Services.Abstract;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetByDateRange(DateOnly startDate, DateOnly endDate);
    Task<IEnumerable<AppointmentSlotDto>> GetSlots(DateOnly startDate, DateOnly endDate);
    Task<IEnumerable<AppointmentDetailsDto>> GetSlotsWithDetails(DateOnly startDate, DateOnly endDate);
    Task<PagedResultDto<AppointmentDto>> GetPaged(int pageNumber, AppointmentStatus status);
    Task<PagedResultDto<AppointmentDto>> GetByIdPaged(int id);
    Task ReceiveGcEventUpdates(string? channelToken);
    Task<AppointmentDto> Deny(int id);
    Task<AppointmentDto> Approve(int id);
    Task<AppointmentDto> Book(BookingDto bookingDto);
    Task<AppointmentDto> Busy(BusyingDto busyingDto);
    Task<AppointmentDto> Update(SubmissionUpdateDto dto);
    AppointmentStatusPropsDto GetAppointmentStatus(AppointmentStatus status);
    IDictionary<AppointmentStatus, AppointmentStatusPropsDto> GetAppointmentStatuses();
}