using BusesControl.Persistence.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BusesControl.Persistence.v1.Repositories;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Entities.Models;
using Microsoft.AspNetCore.Identity;
using BusesControl.Persistence.v1.UnitOfWork;

namespace BusesControl.Persistence;

public class RegisterPersistence
{
    public static void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<IBusRepository, BusRepository>();
        builder.Services.AddScoped<IColorRepository, ColorRepository>();
        builder.Services.AddScoped<IContractRepository, ContractRepository>();
        builder.Services.AddScoped<IContractDescriptionRepository, ContractDescriptionRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<ICustomerContractRepository, CustomerContractRepository>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IResetPasswordSecurityCodeRepository, ResetPasswordSecurityCodeRepository>();
        builder.Services.AddScoped<ISettingPanelRepository, SettingPanelRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserRegistrationQueueRepository, UserRegistrationQueueRepository>();
        builder.Services.AddScoped<IUserRegistrationSecurityCodeRepository, UserRegistrationSecurityCodeRepository>();
        builder.Services.AddScoped<IFinancialRepository, FinancialRepository>();
        builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        builder.Services.AddScoped<ISavedCardRepository, SavedCardRepository>();
        builder.Services.AddScoped<ITerminationRepository, TerminationRepository>();

        builder.Services.AddIdentity<UserModel, IdentityRole<Guid>>(options => 
        {
            options.Password.RequireDigit = true; 
            options.Password.RequireLowercase = true; 
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = true; 
            options.Password.RequiredLength = 10; 
            options.Password.RequiredUniqueChars = 1;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        var conection = builder.Configuration.GetConnectionString("Connection");
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conection));
    }
}
