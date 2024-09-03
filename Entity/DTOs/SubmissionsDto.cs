using Models.Enums;

namespace Models.DTOs
{
    public class SubmissionsDto
    {
        public PagedResultDto<AppointmentDto> PagedResult { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
    }
}
