namespace Models.Models;

public class Customer : Entity
{
    public string phoneNumber { get; set; }
    public string name { get; set; }
    public string surname { get; set; }
    public string email { get; set; }
    public string address { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; }
}