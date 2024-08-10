namespace Models.Models;

public class DoctorAvailability : IEntity
{
    public int id { get; set; }
    public DateTime workStart { get; set; }
    public DateTime workEnd { get; set; }
}