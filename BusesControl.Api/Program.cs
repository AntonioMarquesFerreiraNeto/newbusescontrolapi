using BusesControl.Persistence;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using BusesControl.Business;
using BusesControl.Services;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
using BusesControl.Entities.Validators.v1;

namespace BusesControl.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var appSettingsSection = builder.Configuration.GetRequiredSection("AppSettings");
        builder.Services.Configure<AppSettings>(appSettingsSection);
        var appSettings = appSettingsSection.Get<AppSettings>();

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
        builder.Services.AddSwaggerGen(options => 
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BusesControl API",
                Description = "API para Gerenciamento de contrato de locações frotas de ônibus",
                Version = "v1"
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta forma: Bearer {seu token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        builder.Services.AddValidatorsFromAssemblyContaining<BusCreateRequestValidator>();

        builder.Services.AddHttpContextAccessor();

        RegisterServices.Register(builder);
        RegisterBusiness.Register(builder);
        RegisterPersistence.Register(builder);
        RegisterCommon.Register(builder);

        var key = Encoding.ASCII.GetBytes(appSettings!.JWT.Key);

        builder.Services.AddAuthentication(auth => {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x => {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

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
