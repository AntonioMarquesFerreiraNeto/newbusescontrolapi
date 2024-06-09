using BusesControl.Commons;
using BusesControl.Persistence.v1.Repositories;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Services.v1;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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

        builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(AppSettingsResetPassword.ExpireResetToken);
        });

        builder.Services.AddScoped<IBusService, BusService>();
        builder.Services.AddScoped<IViaCepIntegrationService, ViaCepIntegrationService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserRegistrationQueueService, UserRegistrationQueueService>();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
