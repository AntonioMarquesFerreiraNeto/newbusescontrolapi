using BusesControl.Filters.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class NotificationMiddleware(RequestDelegate _next)
{
    public async Task Invoke(HttpContext context)
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
        var code = StatusCodes.Status500InternalServerError;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;

        return context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
        {
            Title = NotificationTitle.InternalError,
            Detail = exception.Message,
            Status = code,
            Instance = context.Request.Path
        }));
    }
}