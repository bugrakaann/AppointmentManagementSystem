using Business.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Business.Services;

public class GoogleCalendarChannelBackgroundService : BackgroundService
{
    private readonly ILogger<GoogleCalendarChannelBackgroundService> _logger;
    private readonly IGoogleCalendarService _googleCalendarService;
    private readonly string _webhookUrl;
    private string _channelId;
    private string _resourceId;

    public GoogleCalendarChannelBackgroundService(ILogger<GoogleCalendarChannelBackgroundService> logger, IGoogleCalendarService googleCalendarService, IConfiguration configuration)
    {
        _logger = logger;
        _googleCalendarService = googleCalendarService;
        _webhookUrl = configuration["GoogleCalendar:WebhookUrl"] ?? "";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Google Calendar Background Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Eğer bir kanal aktifse önce onu durduruyoruz
                if (!string.IsNullOrEmpty(_channelId) && !string.IsNullOrEmpty(_resourceId))
                {
                    await _googleCalendarService.StopWatching(_channelId, _resourceId);
                    _logger.LogInformation($"Stopped watching calendar. Channel ID: {_channelId}");
                }

                // Yeni kanalı başlatıyoruz
                var channel = await _googleCalendarService.StartWatching(_webhookUrl);
                _channelId = channel.Id;
                _resourceId = channel.ResourceId;

                _logger.LogInformation($"Started watching calendar. Channel ID: {_channelId}");

                // 24 saat bekliyoruz
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while managing Google Calendar watch channels.");
            }
        }

        _logger.LogInformation("Google Calendar Background Service stopped.");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // Servis durdurulduğunda aktif olan kanalı durduruyoruz
        if (!string.IsNullOrEmpty(_channelId) && !string.IsNullOrEmpty(_resourceId))
        {
            await _googleCalendarService.StopWatching(_channelId, _resourceId);
            _logger.LogInformation($"Stopped watching calendar. Channel ID: {_channelId} on service stop.");
        }

        await base.StopAsync(cancellationToken);
    }
}