using BusesControl.Commons.Notification;
using BusesControl.Filters.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class NotificationMiddleware(RequestDelegate _next, ILogger<NotificationMiddleware> _logger)
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

        _logger.LogError("requisição resultou em uma exception, contexto: {0}, detalhes do erro: {1}", context, exception);

        return context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
        {
            Type = $"Método HTTP - {context.Request.Method}",
            Title = NotificationTitle.InternalError,
            Detail = Message.Commons.Unexpected,
            Status = code,
            Instance = context.Request.Path
        }));
    }
}