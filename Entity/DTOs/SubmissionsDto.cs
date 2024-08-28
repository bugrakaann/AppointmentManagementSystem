using Models.Enums;

namespace Models.DTOs
{
    public class SubmissionsDto
    {
        public required PagedResultDto<AppointmentDto> PagedResult { get; set; }
        public required AppointmentStatus AppointmentStatus { get; set; }
    }
}
