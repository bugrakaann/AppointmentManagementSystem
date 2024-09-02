using Models.Models;

namespace Models.DTOs;

public class AppointmentSyncDto
{
    public Appointment Appointment { get; set; }
    public GoogleEventDto? GoogleEvent { get; set; }
}