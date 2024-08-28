using Models.Enums;

namespace Models.DTOs;

public class AppointmentSlotDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AppointmentStatus Status { get; set; }

}