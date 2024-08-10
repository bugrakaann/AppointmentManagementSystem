using DTOs.DTOs;

namespace Services.Services;

public interface IAppointmentService
{
    IEnumerable<AppointmentDto> GetAll();
    AppointmentDto GetById(int id);
    void Add(AppointmentDto appointment);
    void Update(AppointmentDto appointment);
    void Delete(int id);
    void Delete(AppointmentDto appointment);
}