using System.Text.Json;
using BusesControl.Commons.Notification.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BusesControl.Commons.Notification;

public class NotificationFilter : IAsyncResultFilter
{
    private readonly INotificationContext _notificationContext;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public NotificationFilter(INotificationContext notificationContext, JsonSerializerOptions jsonSerializerOptions)
    {
        _notificationContext = notificationContext;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (_notificationContext.HasNotification)
        {
            var code = _notificationContext.StatusCodes;

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = code!.Value;

            ProblemDetails result;
            result = new ProblemDetails
            {
                Type = $"Método HTTP - {context.HttpContext.Request.Method}",
                Title = _notificationContext.Title,
                Detail = _notificationContext.Details,
                Status = _notificationContext.StatusCodes,
                Instance = context.HttpContext.Request.Path
            };
            await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(result, _jsonSerializerOptions));

            return;
        }

        await next();
    }
}