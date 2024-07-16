using BusesControl.Commons;
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
        builder.Services.AddScoped<IColorService, ColorService>();
        builder.Services.AddScoped<IContractService, ContractService>();
        builder.Services.AddScoped<IContractDescriptionService, ContractDescriptionService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<ICustomerContractService, CustomerContractService>();
        builder.Services.AddScoped<IViaCepIntegrationService, ViaCepIntegrationService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<ISettingPanelService, SettingPanelService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserRegistrationQueueService, UserRegistrationQueueService>();
        builder.Services.AddScoped<IGenerationPdfService, GenerationPdfService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IFinancialService, FinancialService>();
        builder.Services.AddScoped<IInvoiceService, InvoiceService>();
        builder.Services.AddScoped<ISavedCardService, SavedCardService>();
        builder.Services.AddScoped<ISystemService, SystemService>();
        builder.Services.AddScoped<IWebhookService, WebhookService>();
        builder.Services.AddScoped<ITerminationService, TerminationService>();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
