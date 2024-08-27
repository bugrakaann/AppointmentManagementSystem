using Models.DTOs;
using Models.Enums;

namespace Models.ViewModel
{
    public class SubmissionsViewModel
    {
        public required PagedResultDto<AppointmentDto> PagedResult { get; set; }
        public required AppointmentStatus AppointmentStatus { get; set; }
    }
}
