using Google.Apis.Calendar.v3.Data;

namespace Business.Services.Abstract;

public interface IGoogleCalendarService
{
    public Task<Event> AddEventAsync(string summary, string description, DateTimeOffset start, DateTimeOffset end);

    public Task<Channel> WatchCalendarAsync(string webhookUrl);

    public Task StopWatchingCalendarAsync(string channelId, string resourceId);

}