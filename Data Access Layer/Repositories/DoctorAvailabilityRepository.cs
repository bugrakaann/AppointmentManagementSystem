using Data_Access_Layer.Data;
using Models.Models;

namespace Data_Access_Layer.Repositories;

public class DoctorAvailabilityRepository : Repository<DoctorAvailability>, IDoctorAvailabilityRepository
{
    public DoctorAvailabilityRepository(ApplicationDbContext context) : base(context)
    {
    }
}