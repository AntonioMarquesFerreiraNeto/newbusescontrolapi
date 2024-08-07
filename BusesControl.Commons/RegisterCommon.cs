﻿using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Filters.Notification;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BusesControl.Commons;

public class RegisterCommon
{
    public static void Register(WebApplicationBuilder builder)
    {         
        builder.Services.AddScoped<INotificationContext, NotificationContext>();
    }
}
