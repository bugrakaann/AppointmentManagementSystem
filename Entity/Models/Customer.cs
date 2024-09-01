namespace Models.Models;

public class Customer : Entity
{
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; }
}