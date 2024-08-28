using Models.DTOs;
using Models.Enums;

namespace Business.Services.Abstract
{
    public interface IUtilService
    {
        string GetHttpErrorMessage(int code);
        AppointmentStatusPropsDto GetAppointmentStatus(AppointmentStatus status);
        IDictionary<AppointmentStatus, AppointmentStatusPropsDto> GetAppointmentStatuses();
        string GetJson(object obj);
    }
}