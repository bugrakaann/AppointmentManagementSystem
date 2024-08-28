using Business.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Models.DTOs;
using Models.Enums;

namespace Business.Services;

public class UtilService : IUtilService
{

    public UtilService()
    {
    }

    public string GetHttpErrorMessage(int code)
    {
        var messages = new Dictionary<int, string>
        {
            { 400, "Geçersiz istek!" },
            { 401, "Yetkisiz erişim!" },
            { 403, "Erişim engellendi!" },
            { 404, "Sayfa bulunamadı!" },
            { 500, "Sunucu hatası!" },
            { 502, "Sunucudan geçersiz yanıt!" },
            { 503, "Sunucu kullanılamıyor!" },
            { 504, "Ağ geçidi zaman aşımına uğradı" }
        };
        try
        {
            return messages[code];
        }
        catch
        {
            return "Bir hata oluştu!";
        }
    }

    public AppointmentStatusPropsDto GetAppointmentStatus(AppointmentStatus status)
    {
        var statuses = GetAppointmentStatuses();
        return statuses[status];
    }

    public IDictionary<AppointmentStatus, AppointmentStatusPropsDto> GetAppointmentStatuses()
    {
        return new Dictionary<AppointmentStatus, AppointmentStatusPropsDto>
        {
            {
                AppointmentStatus.Busy,
                new AppointmentStatusPropsDto
                {
                    Status = AppointmentStatus.Busy,
                    ColorId = "5",
                    ColorCode = "orange",
                    Title = "MÜSAİT DEĞİL",
                    IsValid = true
                }
            },
            {
                AppointmentStatus.WaitingForApproval,
                new AppointmentStatusPropsDto
                {
                    Status = AppointmentStatus.WaitingForApproval,
                    ColorId = "8",
                    ColorCode = "gray",
                    Title = "MEŞGUL",
                    IsValid = true
                }
            },
            {
                AppointmentStatus.Approved,
                new AppointmentStatusPropsDto
                {
                    Status = AppointmentStatus.Approved,
                    ColorId = "4",
                    ColorCode = "red",
                    Title = "REZERVE",
                    IsValid = true
                }
            },
            {
                AppointmentStatus.Denied,
                new AppointmentStatusPropsDto
                {
                    Status = AppointmentStatus.Denied,
                    ColorId = "9",
                    ColorCode = "blue",
                    Title = "REDDEDİLDİ",
                    IsValid = false
                }
            }
        };
    }

    public string GetJson(object obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj);
    }

}