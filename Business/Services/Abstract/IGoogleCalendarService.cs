using Google.Apis.Calendar.v3.Data;
using Models.DTOs;

namespace Business.Services.Abstract;

public interface IGoogleCalendarService
{
    string CalendarId { get; }
    string CalendarToken { get; }
    Task<GoogleCalendarEventDto> AddEvent(GoogleCalendarEventDto dto);
    Task DeleteEvent(string eventId);
    Task<GoogleCalendarEventDto> GetEvent(string eventId);
    Task<GoogleCalendarEventDto> UpdateEventColor(string eventId, string colorId);
    Task<GoogleCalendarEventDto> UpdateEvent(GoogleCalendarEventDto dto);
     Task<Channel> StartWatching(string webhookUrl);
     Task StopWatching(string channelId, string resourceId);
     Task<IEnumerable<GoogleCalendarEventDto>> GetUpdatedEvents();
}