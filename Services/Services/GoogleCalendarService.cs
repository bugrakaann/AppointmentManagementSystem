using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;

namespace Services.Services
{
    public class GoogleCalendarService : ICalendarService
    {
        private readonly CalendarService _calendarService;
        private readonly string calendarId;

        public GoogleCalendarService(IConfiguration configuration)
        {
            var serviceAccountKeyFilePath = configuration["GoogleCalendar:ServiceAccountKeyFilePath"];

            if (string.IsNullOrEmpty(serviceAccountKeyFilePath))
            {
                throw new Exception("Google Service Account key file path is not configured properly.");
            }

            GoogleCredential credential;
            using (var stream = new FileStream(serviceAccountKeyFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(CalendarService.Scope.Calendar);
            }

            _calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "AppointmentSystem",
            });


            this.calendarId = configuration["GoogleCalendar:CalendarID"] ?? "primary";
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
            var request = _calendarService.Events.Insert(newEvent, this.calendarId);
            return await request.ExecuteAsync();
        }
    }
}