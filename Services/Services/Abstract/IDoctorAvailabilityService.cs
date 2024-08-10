using DTOs.DTOs;

namespace Services.Services;

public interface IDoctorAvailabilityService
{
    Dictionary<DateTime,IEnumerable<DoctorAvailabilityDto>> GetAll();
    DoctorAvailabilityDto GetById(int id);
    void Add(DoctorAvailabilityDto customer);
    void Update(DoctorAvailabilityDto customer);
    void Delete(int id);
    void Delete(DoctorAvailabilityDto customer);
    public List<DoctorAvailabilityDto> GetByDate(DateTime date);
}