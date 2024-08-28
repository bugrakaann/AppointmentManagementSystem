using Models.Enums;

namespace Models.Models;

public class Appointment : Entity
{
    public int? customerId { get; set; }
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public string description { get; set; }
    public AppointmentStatus status { get; set; }
    public Customer? customer { get; set; }

}