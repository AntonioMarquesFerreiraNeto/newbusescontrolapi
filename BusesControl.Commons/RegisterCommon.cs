using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Filters.Notification;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BusesControl.Commons;

public static class RegisterCommon
{
    public static void ExecuteRegisterCommon(this WebApplicationBuilder builder)
    {         
        builder.Services.AddScoped<INotificationContext, NotificationContext>();
    }
}
