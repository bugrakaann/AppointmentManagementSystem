using Models.Enums;

namespace Models.DTOs;

public class AppointmentDto
{
    public CustomerDto? Customer { get; set; }
    public int? customerId { get; set; }
    public int Id { get; set; }
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public string description { get; set; }
    public AppointmentStatus status { get; set; }

    public CustomerDto CustomerDto { get; set; }


    public void FlushCustomer()
    {
        Customer = null;
        customerId = null;
        description = "";
    }
}