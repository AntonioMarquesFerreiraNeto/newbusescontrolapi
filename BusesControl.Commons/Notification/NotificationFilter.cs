using System.Text.Json;
using BusesControl.Commons.Notification.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BusesControl.Commons.Notification;

public class NotificationFilter : IAsyncResultFilter
{
    private readonly INotificationApi _notificationApi;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public NotificationFilter(INotificationApi notificationApi, JsonSerializerOptions jsonSerializerOptions)
    {
        _notificationApi = notificationApi;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (_notificationApi.HasNotification)
        {
            var code = _notificationApi.StatusCodes;

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = code!.Value;

            ProblemDetails result;
            result = new ProblemDetails
            {
                Title = _notificationApi.Title,
                Detail = _notificationApi.Details,
                Status = _notificationApi.StatusCodes,
                Instance = context.HttpContext.Request.Path,
            };
            await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(result, _jsonSerializerOptions));

            return;
        }

        await next();
    }
}