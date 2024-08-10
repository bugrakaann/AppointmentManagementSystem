using AutoMapper;
using DTOs.DTOs;
using Models.Models;

namespace Profiles.Profiles;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
        CreateMap<DoctorAvailability, DoctorAvailabilityDto>().ReverseMap(); 
    }
}