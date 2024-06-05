using BusesControl.Commons;
using BusesControl.Services.v1;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace BusesControl.Services;

public class RegisterServices
{
    public static void Register(WebApplicationBuilder builder)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        builder.Services.AddSingleton(jsonSerializerOptions);

        builder.Services.AddScoped<AppSettings>();

        builder.Services.AddScoped<IBusService, BusService>();
        builder.Services.AddScoped<IViaCepIntegrationService, ViaCepIntegrationService>();
    }
}
