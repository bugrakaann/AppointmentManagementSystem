using AppointmentManagementSystem.Models;

namespace AppointmentManagementSystem.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    IEnumerable<Appointment> GetAll();
    Appointment GetById(int id);
    void Add(Appointment appointment);
    void Update(Appointment appointment);
    void Delete(int id);
    void Delete(Appointment appointment);
}