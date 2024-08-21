using System.Linq.Expressions;
using DTOs.DTOs;
using Models.Enums;

namespace Services.Services;

public interface IAppointmentService
{
    IEnumerable<AppointmentDto> GetAll();
    IEnumerable<AppointmentDto> GetAll(AppointmentStatus status);
    IEnumerable<AppointmentDto> GetRange(int startIndex, int count);
    IEnumerable<AppointmentDto> GetRange(AppointmentStatus category, int startIndex, int count);
    AppointmentDto GetById(int id);
    void Add(AppointmentDto appointment);
    void Update(AppointmentDto appointment);
    void Delete(int id);
    void Delete(AppointmentDto appointment);
    bool CheckExistingSession(AppointmentDto dto);
    int GetAppointmentNumber();
    int GetCountByStatus(AppointmentStatus status);
    IEnumerable<AppointmentDto> GetByDateRange(DateOnly startTime, DateOnly endTime);
}