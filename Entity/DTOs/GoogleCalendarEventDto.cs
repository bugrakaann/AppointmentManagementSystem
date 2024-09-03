namespace Models.DTOs;

public class GoogleCalendarEventDto
{
    public string Id { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string ColorId { get; set; }
    public string Status { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
}