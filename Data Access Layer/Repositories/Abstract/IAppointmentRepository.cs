using Models.Enums;
using Models.Models;

namespace Data_Access_Layer.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    IEnumerable<Appointment> GetRangeByStatus(AppointmentStatus status, int startIndex, int count);
    IEnumerable<Appointment> GetRange(int startIndex, int count);
    int GetAppointmentNumber();
    int GetCountByStatus(AppointmentStatus status);
    IEnumerable<Appointment> GetByDateRange(DateTime startTime, DateTime endTime);
}