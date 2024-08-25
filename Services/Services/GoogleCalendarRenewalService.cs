using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Services.Services;

public class GoogleCalendarRenewalService : BackgroundService
{
    private readonly GoogleCalendarService _googleCalendarService;
    private readonly ILogger<GoogleCalendarRenewalService> _logger;
    private readonly IConfiguration _configuration;

    // Kanal bilgilerini burada saklıyoruz
    private string _channelId;
    private string _resourceId;

    public GoogleCalendarRenewalService(GoogleCalendarService googleCalendarService, ILogger<GoogleCalendarRenewalService> logger, IConfiguration configuration)
    {
        _googleCalendarService = googleCalendarService;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("GoogleCalendarRenewalService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Webhook URL'sini config'den alıyoruz
                var webhookUrl = _configuration["GoogleCalendar:WebhookUrl"];

                // Eğer bir kanal aktifse önce onu durduruyoruz
                if (!string.IsNullOrEmpty(_channelId) && !string.IsNullOrEmpty(_resourceId))
                {
                    await _googleCalendarService.StopWatchingCalendarAsync(_channelId, _resourceId);
                    _logger.LogInformation($"Stopped channel {_channelId}");
                }

                // Yeni kanalı başlatıyoruz
                var newChannel = await _googleCalendarService.WatchCalendarAsync(webhookUrl);
                _channelId = newChannel.Id;
                _resourceId = newChannel.ResourceId;

                _logger.LogInformation($"Started new channel {_channelId}");

                // 24 saat bekliyoruz
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while renewing the Google Calendar channel.");
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // Servis durdurulduğunda aktif olan kanalı durduruyoruz
        if (!string.IsNullOrEmpty(_channelId) && !string.IsNullOrEmpty(_resourceId))
        {
            await _googleCalendarService.StopWatchingCalendarAsync(_channelId, _resourceId);
            _logger.LogInformation($"Stopped channel {_channelId} on service stop.");
        }

        await base.StopAsync(cancellationToken);
    }
}
