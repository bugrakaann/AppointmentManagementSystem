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

    private Event EventBody(GoogleCalendarEventDto dto)
    {
        return new Event()
        {
            Summary = dto.Summary,
            Description = dto.Description,
            ColorId = dto.ColorId,
            Start = new EventDateTime()
            {
                DateTimeDateTimeOffset = dto.StartTime,
                TimeZone = TimeZoneInfo.Local.ToString()
            },
            End = new EventDateTime()
            {
                DateTimeDateTimeOffset = dto.EndTime,
                TimeZone = TimeZoneInfo.Local.ToString()
            },
        };
    }

    public async Task<GoogleCalendarEventDto> AddEvent(GoogleCalendarEventDto dto)
    {
        var eventBody = EventBody(dto);
        var result = await _calendarService.Events.Insert(eventBody, CalendarId).ExecuteAsync();
        return _mapper.Map<GoogleCalendarEventDto>(result);
    }

    public async Task DeleteEvent(string eventId)
    {
        await _calendarService.Events.Delete(CalendarId, eventId).ExecuteAsync();
    }

    private async Task<Event> GetEventData(string eventId)
    {
        return await _calendarService.Events.Get(CalendarId, eventId).ExecuteAsync();
    }

    public async Task<GoogleCalendarEventDto> GetEvent(string eventId)
    {
        var result = await GetEventData(eventId);
        return _mapper.Map<GoogleCalendarEventDto>(result);
    }

    public async Task<IEnumerable<GoogleCalendarEventDto>> GetUpdatedEvents()
    {
        var q = _calendarService.Events.List(CalendarId);
        q.TimeZone = TimeZoneInfo.Local.ToString();
        q.UpdatedMinDateTimeOffset = DateTimeOffset.Now.AddDays(-14);
        q.MaxResults = 2500;
        var result = await q.ExecuteAsync();
        return result.Items.Reverse().Select(e => _mapper.Map<GoogleCalendarEventDto>(e));
    }

    public async Task<GoogleCalendarEventDto> UpdateEventColor(string eventId, string colorId)
    {
        var eventBody = new Event
        {
            ColorId = colorId,
        };
        var result = await _calendarService.Events.Patch(eventBody, CalendarId, eventId).ExecuteAsync();
        return _mapper.Map<GoogleCalendarEventDto>(result);
    }

    public async Task<GoogleCalendarEventDto> UpdateEvent(GoogleCalendarEventDto dto)
    {
        var eventBody = EventBody(dto);
        var result = await _calendarService.Events.Patch(eventBody, CalendarId, dto.Id).ExecuteAsync();
        return _mapper.Map<GoogleCalendarEventDto>(result);
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
        return await _calendarService.Events.Watch(channel, CalendarId).ExecuteAsync();
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