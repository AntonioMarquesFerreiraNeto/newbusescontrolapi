using BusesControl.Persistence;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using BusesControl.Business;
using BusesControl.Services;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using FluentValidation;
using System.Text;
using BusesControl.Entities.Validators.v1;
using BusesControl.Api.Utils;
using BusesControl.Api.Extensions;

namespace BusesControl.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var appSettingsSection = builder.Configuration.GetRequiredSection("AppSettings");
        builder.Services.Configure<AppSettings>(appSettingsSection);
        var appSettings = appSettingsSection.Get<AppSettings>();

        builder.Services.AddControllers(options => 
        {
            options.ModelValidatorProviders.Clear();
            options.Filters.Add(new ConsumesAttribute("application/json"));
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.Filters.Add<NotificationFilter>();
        }).AddJsonOptions(options => 
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        builder.Services.AddSignalR();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.RegisterSwagger();

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        builder.Services.AddValidatorsFromAssemblyContaining<BusCreateRequestValidator>();

        builder.Services.AddHttpContextAccessor();

        var privateJwtKey = Encoding.ASCII.GetBytes(appSettings!.JWT.Key);

        builder.ExecuteRegisterServices();
        builder.ExecuteRegisterBusiness();
        builder.ExecuteRegisterPersistence();
        builder.ExecuteRegisterCommon();

        builder.Services.RegisterAuthentication(privateJwtKey);
        builder.Services.RegisterRateLimiter();

        var app = builder.Build();

        app.UseRateLimiter();

        app.UseMiddleware<NotificationMiddleware>();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        var allowedOrigins = builder.Configuration.GetSection("AppSettings:Cors:AllowedOrigins").Get<string[]>() ?? [];

        app.UseCors(x => {
            x.WithOrigins(allowedOrigins);
            x.AllowAnyMethod();
            x.AllowAnyHeader();
            x.AllowCredentials();
        });

        app.UseAuthorization();

        app.MapHub<SupportChatHub>("ws/support");

        app.MapControllers();

        await app.RunAsync();
    }
}
