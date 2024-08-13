using Models.Models;
using Models.Enums;

namespace DTOs.DTOs;

public class AppointmentDto
{
    public CustomerDto Customer { get; set; }
    public int? customerId { get; set; }
    public int Id { get; set; }
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public string description { get; set; }
    public AppointmentStatus status { get; set; }

    public void SetStatus(AppointmentStatus status)
    {
        this.status = status;
    }
    public void SetCustomerId(CustomerDto customer)
    {
        customerId = customer.id;
    }
    public void SetCustomer(CustomerDto customer)
    {
        Customer = customer;
    }
    public void FlushCustomer()
    {
        Customer = null;
        customerId = null;
        description = "Empty";
    }
}