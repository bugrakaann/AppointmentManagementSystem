using Models.Enums;

namespace Models.Models;

public class Appointment : IEntity
{
    public int? customerId { get; set; }
    public Customer customer { get; set; }
    public int id { get; set; }
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public string description { get; set; }
    public AppointmentStatus status { get; set; }
}