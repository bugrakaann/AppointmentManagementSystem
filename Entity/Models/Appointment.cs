using Models.Enums;

namespace Models.Models;

public class Appointment : Entity
{
    public int? CustomerId { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? GoogleEventId { get; set; }
    public Customer? Customer { get; set; }
}