using AutoMapper;
using AppointmentManagementSystem.Models;
using AppointmentManagementSystem.DTOs;

namespace AppointmentManagementSystem.Profiles;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
    }
}