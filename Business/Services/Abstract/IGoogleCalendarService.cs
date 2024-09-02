using Google.Apis.Calendar.v3.Data;
using Models.DTOs;

namespace Business.Services.Abstract;

public interface IGoogleCalendarService
{
    string CalendarId { get; }
    string CalendarToken { get; }
    Task<GoogleEventDto> AddEvent(string title, string description, DateTimeOffset start, DateTimeOffset end, string colorId);
    Task DeleteEvent(string eventId);
    Task<GoogleEventDto> GetEvent(string eventId);
    Task<GoogleEventDto> UpdateEventColor(string eventId, string colorId);
    public Task<Channel> StartWatching(string webhookUrl);
    public Task StopWatching(string channelId, string resourceId);
}