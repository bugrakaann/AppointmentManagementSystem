using Google.Apis.Calendar.v3.Data;

namespace Business.Services.Abstract;

public interface IGoogleCalendarService
{
    Task<Event> AddEvent(string summary, string description, DateTimeOffset start, DateTimeOffset end, string colorId);

    Task<Channel> WatchCalendarAsync(string webhookUrl);

    Task StopWatchingCalendarAsync(string channelId, string resourceId);
}