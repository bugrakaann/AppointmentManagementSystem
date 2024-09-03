using Models.DTOs;
using Models.Enums;

namespace Business.Services.Abstract
{
    public interface IUtilService
    {
        string GetHttpErrorMessage(int code);
        string GetJson(object obj);
        string UrlToAction(string actionName, string controllerName, object routeValues = null);
        DateTime DateTimeOffsetToDateTime(DateTimeOffset dateTimeOffset);
    }
}