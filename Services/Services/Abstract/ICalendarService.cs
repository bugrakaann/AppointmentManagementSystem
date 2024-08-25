namespace Services.Services;

public interface ICalendarService
{
    public Task<Google.Apis.Calendar.v3.Data.Event> AddEventAsync(string summary, string description, DateTimeOffset start, DateTimeOffset end);

}