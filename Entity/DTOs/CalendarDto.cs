namespace Models.DTOs;

public class CalendarDto
{
    public string AppointmentStatuses { get; set; }
    public BookingDto? Booking { get; set; }
}