using BusesControl.Commons.Notification;
using BusesControl.Filters.Notification;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;

namespace BusesControl.Api.Extensions
{
    public static class RateLimiter
    {
        public static void RegisterRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options => {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.OnRejected = async (context, token) => {
                    await context.HttpContext.Response.WriteAsJsonAsync(new ProblemDetails
                    {
                        Type = $"Método HTTP - {context.HttpContext.Request.Method}",
                        Title = NotificationTitle.TooManyRequests,
                        Detail = Message.Commons.TooManyRequests,
                        Status = StatusCodes.Status429TooManyRequests,
                        Instance = context.HttpContext.Request.Path
                    }, cancellationToken: token);
                };
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 200,
                            QueueLimit = 0,
                            Window = TimeSpan.FromMinutes(1)
                        }
                    )
                );
                options.AddPolicy("auth-policy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            QueueLimit = 0,
                            Window = TimeSpan.FromMinutes(1),
                            AutoReplenishment = true
                        }
                    )
                );
                options.AddPolicy("two-fa-policy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 20,
                            QueueLimit = 0,
                            Window = TimeSpan.FromMinutes(1),
                            AutoReplenishment = true
                        }
                    )
                );
            });
        }
    }
}
