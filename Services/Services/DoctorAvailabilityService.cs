using AutoMapper;
using Data_Access_Layer.Repositories;
using DTOs.DTOs;
using Models.Models;

namespace Services.Services;

public class DoctorAvailabilityService : IDoctorAvailabilityService
{
    public readonly IMapper _mapper;

    IDoctorAvailabilityRepository _doctorAvailabilityRepository;

    public DoctorAvailabilityService(IDoctorAvailabilityRepository doctorAvailabilityRepository, IMapper mapper)
    {
        _doctorAvailabilityRepository = doctorAvailabilityRepository;
        _mapper = mapper;
    }
    
    public Dictionary<DateTime,IEnumerable<DoctorAvailabilityDto>> GetAll()
    {
        var availabilities = _doctorAvailabilityRepository.GetAll();
        var filtered = availabilities.Where(a => a.workStart > DateTime.Now && a.workEnd > DateTime.Now);
        
        var grouped = filtered.GroupBy(a => a.workStart.Date).ToDictionary(g => g.Key, g => g.AsEnumerable());

         return _mapper.Map<Dictionary<DateTime,IEnumerable<DoctorAvailabilityDto>>>(grouped);
    }

    public List<DoctorAvailabilityDto> GetByDate(DateTime date)
    {
        var availabilities = _doctorAvailabilityRepository.GetAll();
        var filtered = availabilities.Where(a => a.workStart.Date == date.Date);

         return _mapper.Map<List<DoctorAvailabilityDto>>(filtered);
    }

    public DoctorAvailabilityDto GetById(int id)
    {
        var availabilities = _doctorAvailabilityRepository.GetById(id);
        return _mapper.Map<DoctorAvailabilityDto>(availabilities);
    }

    public void Add(DoctorAvailabilityDto availabilityDto)
{
    var availability = _mapper.Map<DoctorAvailability>(availabilityDto);

    var timeSlots = GenerateTimeSlots(availability.workStart, availability.workEnd);

    foreach (var slot in timeSlots)
    {
        var newAvailability = new DoctorAvailability
        {
            workStart = slot.Item1,
            workEnd = slot.Item2
        };

        _doctorAvailabilityRepository.Add(newAvailability);
    }
}

private List<(DateTime, DateTime)> GenerateTimeSlots(DateTime workStart, DateTime workEnd)
{
    var slots = new List<(DateTime, DateTime)>();

    DateTime currentStart = workStart;
    while (currentStart < workEnd)
    {
        DateTime currentEnd = currentStart.AddMinutes(30);
        if (currentEnd <= workEnd)
        {
            slots.Add((currentStart, currentEnd));
        }
        currentStart = currentEnd;
    }

    return slots;
}

    public void Update(DoctorAvailabilityDto availabilityDto)
    {
        var availability = _mapper.Map<DoctorAvailability>(availabilityDto);
        _doctorAvailabilityRepository.Update(availability);
    }

    public void Delete(int id)
    {
        _doctorAvailabilityRepository.Delete(id);
    }

    public void Delete(DoctorAvailabilityDto availabilityDto)
    {
         var availability = _mapper.Map<DoctorAvailability>(availabilityDto);
         _doctorAvailabilityRepository.Delete(availability);
    }
}