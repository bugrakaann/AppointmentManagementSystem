using AutoMapper;
using Models.DTOs;
using Models.Models;

namespace Business.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
    }
}