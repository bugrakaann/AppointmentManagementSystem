using Business.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Models.DTOs;
using Models.Enums;

namespace Business.Services;

public class UtilService : IUtilService
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UtilService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
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

    public string GetJson(object obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj);
    }

    public string UrlToAction(string actionName, string controllerName, object routeValues = null)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var url = _linkGenerator.GetPathByAction(httpContext, actionName, controllerName, routeValues);
        return url;
    }

}