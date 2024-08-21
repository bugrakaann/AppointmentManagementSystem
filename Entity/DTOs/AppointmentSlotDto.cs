using Models.Models;
using Models.Enums;

namespace DTOs.DTOs;

public class AppointmentSlotDto
{

    public int Id { get; set; }
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public AppointmentStatus status { get; set; }

}