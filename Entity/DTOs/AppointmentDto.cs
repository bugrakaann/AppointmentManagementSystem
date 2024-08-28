using Models.Enums;

namespace Models.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public AppointmentStatus Status { get; set; }
    public CustomerDto Customer { get; set; }
}