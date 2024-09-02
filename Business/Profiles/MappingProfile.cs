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
        CreateMap<AppointmentDto, AppointmentSlotDto>().ReverseMap();
        CreateMap<AppointmentDto, AppointmentDetailsDto>().ReverseMap();
        CreateMap<Google.Apis.Calendar.v3.Data.Event, GoogleEventDto>().ReverseMap();
    }
}