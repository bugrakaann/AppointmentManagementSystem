using AutoMapper;
using Business.Services.Abstract;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Models.DTOs;

namespace Business.Services;

public class GoogleCalendarService : IGoogleCalendarService
{
    private readonly IMapper _mapper;
    private readonly CalendarService _calendarService;

    public string CalendarId { get; }

    public string CalendarToken => $"TOKEN_{CalendarId}";

    public GoogleCalendarService(IMapper mapper, IConfiguration configuration)
    {
        var serviceAccountKeyFilePath = configuration["GoogleCalendar:ServiceAccountKeyFilePath"];
        CalendarId = configuration["GoogleCalendar:CalendarID"] ?? "";
        _calendarService = Init(serviceAccountKeyFilePath);
        _mapper = mapper;
    }

    private CalendarService Init(string? serviceAccountKeyFilePath)
    {
        if (string.IsNullOrEmpty(CalendarId))
        {
            throw new Exception("Takvim ID eksik");
        }

        if (string.IsNullOrEmpty(serviceAccountKeyFilePath))
        {
            throw new Exception("Google servis hesabı eksik");
        }

        GoogleCredential credential;
        using (var stream = new FileStream(serviceAccountKeyFilePath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped(CalendarService.Scope.Calendar);
        }

        return new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
        });
    }

    public async Task<GoogleCalendarEventDto> AddEvent(string summary, string description, DateTimeOffset start,
        DateTimeOffset end,
        string colorId)
    {
        var newEvent = new Event()
        {
            Summary = summary,
            Description = description,
            ColorId = colorId,
            Start = new EventDateTime()
            {
                DateTimeDateTimeOffset = start,
                TimeZone = TimeZoneInfo.Local.ToString()
            },
            End = new EventDateTime()
            {
                DateTimeDateTimeOffset = end,
                TimeZone = TimeZoneInfo.Local.ToString()
            },
        };
        var request = await _calendarService.Events.Insert(newEvent, CalendarId).ExecuteAsync();
        return _mapper.Map<GoogleCalendarEventDto>(request);
    }

    public async Task DeleteEvent(string eventId)
    {
        var deleteRequest = _calendarService.Events.Delete(CalendarId, eventId);
        await deleteRequest.ExecuteAsync();
    }

    private async Task<Event> GetEventData(string eventId)
    {
        return await _calendarService.Events.Get(CalendarId, eventId).ExecuteAsync();
    }

    public async Task<GoogleCalendarEventDto> GetEvent(string eventId)
    {
        var request = await GetEventData(eventId);
        return _mapper.Map<GoogleCalendarEventDto>(request);
    }

    public async Task<IEnumerable<GoogleCalendarEventDto>> GetUpdatedEvents()
    {
        var q = _calendarService.Events.List(CalendarId);
        q.TimeZone = TimeZoneInfo.Local.ToString();
        q.UpdatedMinDateTimeOffset = DateTimeOffset.Now.AddDays(-29);
        q.ShowDeleted = true;
        q.MaxResults = 2500;
        var events = await q.ExecuteAsync();
        return events.Items.Reverse().Select(e => _mapper.Map<GoogleCalendarEventDto>(e));
    }

    public async Task<GoogleCalendarEventDto> UpdateEventColor(string eventId, string colorId)
    {
        var existingEvent = await GetEventData(eventId);
        existingEvent.ColorId = colorId;
        var request = await _calendarService.Events.Update(existingEvent, CalendarId, eventId).ExecuteAsync();
        return _mapper.Map<GoogleCalendarEventDto>(request);
    }

    public async Task<Channel> StartWatching(string webhookUrl)
    {
        var channel = new Channel()
        {
            Id = Guid.NewGuid().ToString(),
            Type = "web_hook",
            Address = webhookUrl,
            Token = CalendarToken
        };

        var watchRequest = _calendarService.Events.Watch(channel, CalendarId);
        return await watchRequest.ExecuteAsync();
    }

    public async Task StopWatching(string channelId, string resourceId)
    {
        var channel = new Channel()
        {
            Id = channelId,
            ResourceId = resourceId
        };
        await _calendarService.Channels.Stop(channel).ExecuteAsync();
    }
    
}