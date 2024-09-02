using Models.Enums;
using Models.Models;

namespace Data_Access_Layer.Repositories.Abstract;

public interface IAppointmentRepository : IRepository<Appointment>
{
    AppointmentStatus[] ValidStatuses { get; set; }
    Task<IEnumerable<Appointment>> GetRangeByStatus(AppointmentStatus status, int startIndex, int count);
    Task<int> GetCountByStatus(AppointmentStatus status);
    Task<IEnumerable<Appointment>> GetByDateRange(DateTime startTime, DateTime endTime, AppointmentStatus[] statuses);
    Task<bool> IsOverlapping(DateTime startTime, DateTime endTime);
}