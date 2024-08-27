using Models.DTOs;

namespace Models.ViewModel;
public class AppointmentViewModel
{
    public IEnumerable<AppointmentDto> Appointments { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int CategoryId { get; set; }
}
