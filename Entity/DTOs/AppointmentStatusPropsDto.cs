using Models.Enums;

namespace Models.DTOs
{
    public class AppointmentStatusPropsDto
    {
        public AppointmentStatus Status { get; set; }
        public string Title { get; set; }
        public bool IsValid { get; set; }
        public string ColorId { get; set; }
        public string ColorCode { get; set; }
        public int StatusId => (int)Status;
        public string StatusCode => Status.ToString();
    }
}