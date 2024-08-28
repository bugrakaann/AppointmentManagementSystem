using Business.Services.Abstract;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;

namespace Business.Services
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly CalendarService _calendarService;
        private readonly string _calendarId;

        public GoogleCalendarService(IConfiguration configuration)
        {
            var serviceAccountKeyFilePath = configuration["GoogleCalendar:ServiceAccountKeyFilePath"];
            _calendarId = configuration["GoogleCalendar:CalendarID"] ?? "";
            _calendarService = InitializeCalendarService(serviceAccountKeyFilePath);
        }

        private CalendarService InitializeCalendarService(string? serviceAccountKeyFilePath)
        {
            if (string.IsNullOrEmpty(_calendarId))
            {
                throw new Exception("Takvim ID eksik");
            }

            if (string.IsNullOrEmpty(serviceAccountKeyFilePath))
            {
                throw new Exception("Google servis hesabý eksik");
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

        public async Task<Event> AddEventAsync(string summary, string description, DateTimeOffset start, DateTimeOffset end)
        {
            var newEvent = new Event()
            {
                Summary = summary,
                Description = description,
                ColorId = "8",
                Start = new EventDateTime()
                {
                    DateTimeDateTimeOffset = start.UtcDateTime,
                    TimeZone = "UTC",
                },
                End = new EventDateTime()
                {
                    DateTimeDateTimeOffset = end.UtcDateTime,
                    TimeZone = "UTC",
                },
            };
            var request = _calendarService.Events.Insert(newEvent, _calendarId);
            return await request.ExecuteAsync();
        }


        public async Task<Channel> WatchCalendarAsync(string webhookUrl)
        {
            var channel = new Channel()
            {
                Id = Guid.NewGuid().ToString(),
                Type = "web_hook",
                Address = webhookUrl
            };

            var watchRequest = _calendarService.Events.Watch(channel, _calendarId);
            return await watchRequest.ExecuteAsync();
        }

        public async Task StopWatchingCalendarAsync(string channelId, string resourceId)
        {
            var channel = new Channel()
            {
                Id = channelId,
                ResourceId = resourceId
            };

            await _calendarService.Channels.Stop(channel).ExecuteAsync();
        }

    }
}