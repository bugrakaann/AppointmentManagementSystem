using Google.Apis.Calendar.v3.Data;

namespace Services.Services;

public interface ICalendarService
{
    public Task<Google.Apis.Calendar.v3.Data.Event> AddEventAsync(string summary, string description, DateTimeOffset start, DateTimeOffset end);

    public Task<Channel> WatchCalendarAsync(string webhookUrl);

    public Task StopWatchingCalendarAsync(string channelId, string resourceId);

}