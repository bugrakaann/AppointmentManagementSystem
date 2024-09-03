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
        CreateMap<Google.Apis.Calendar.v3.Data.Event, GoogleCalendarEventDto>()
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Start.DateTimeDateTimeOffset))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.End.DateTimeDateTimeOffset))
            .ReverseMap()
            .ForPath(src => src.Start.DateTimeDateTimeOffset, opt => opt.MapFrom(dest => dest.StartTime))
            .ForPath(src => src.End.DateTimeDateTimeOffset, opt => opt.MapFrom(dest => dest.EndTime));
    }
}