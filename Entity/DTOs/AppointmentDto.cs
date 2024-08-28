using Models.Enums;

namespace Models.DTOs;

public class AppointmentDto : AppointmentSlotDto
{
    public string Description { get; set; }
    public CustomerDto Customer { get; set; }
}