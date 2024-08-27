using System.Net;

namespace AppointmentManagementSystem.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.Clear();
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.Redirect(context.Request.PathBase + "/Home/Error");
        return Task.CompletedTask;
    }
}