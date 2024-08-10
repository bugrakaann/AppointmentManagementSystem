using Data_Access_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Data_Access_Layer.Repositories;

public class AppointmentRepository : Repository<Appointment>,IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
    }
}