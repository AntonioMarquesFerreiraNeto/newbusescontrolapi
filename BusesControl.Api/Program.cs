using BusesControl.Persistence;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using BusesControl.Business;
using BusesControl.Services;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using FluentValidation;
using BusesControl.Entities.Validators;

namespace BusesControl.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
        .AddControllers(options => {
            options.ModelValidatorProviders.Clear();
            options.Filters.Add(new ConsumesAttribute("application/json"));
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.Filters.Add<NotificationFilter>();
        })

        .AddJsonOptions(options => {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        builder.Services.AddValidatorsFromAssemblyContaining<BusCreateRequestValidator>();

        builder.Services.AddHttpContextAccessor();

        RegisterServices.Register(builder);
        RegisterBusiness.Register(builder);
        RegisterPersistence.Register(builder);
        RegisterCommon.Register(builder);

        var app = builder.Build();
        
        app.UseMiddleware<NotificationMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
