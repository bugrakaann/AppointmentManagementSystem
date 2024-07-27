using AppointmentManagementSystem.Models;

namespace AppointmentManagementSystem.DTOs;

public class AppointmentDto
{
    public int customerId { get; set; }
    public Customer customer { get; set; }
    public int Id { get; set; }
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public string description { get; set; }
}