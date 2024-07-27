using Data_Access_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Data_Access_Layer.Repositories;

public class AppointmentRepository : Repository<Appointment>,IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    
    public IEnumerable<Appointment> GetAll()
    {
        return _context.Appointments.Include(a=> a.customer).ToList();
    }

    public Appointment GetById(int id)
    {
        return _context.Appointments.Include(a => a.customer).FirstOrDefault(a => a.id == id);
    }

    public void Add(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        _context.SaveChanges();
    }

    public void Update(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        Appointment appointment = _context.Appointments.FirstOrDefault(a=>a.id == id);
        if(appointment == null) return;
        _context.Appointments.Remove(appointment);
        _context.SaveChanges();
    }

    public void Delete(Appointment appointment)
    {
        _context.Appointments.Remove(appointment);
        _context.SaveChanges();
    }
}