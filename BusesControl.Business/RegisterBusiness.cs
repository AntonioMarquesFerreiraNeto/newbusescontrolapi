﻿using BusesControl.Business.v1;
using BusesControl.Business.v1.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BusesControl.Business;

public class RegisterBusiness
{
    public static void Register(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBusBusiness, BusBusiness>();
        builder.Services.AddScoped<IColorBusiness, ColorBusiness>();
        builder.Services.AddScoped<IContractBusiness, ContractBusiness>();
        builder.Services.AddScoped<IContractDescriptionBusiness, ContractDescriptionBusiness>();
        builder.Services.AddScoped<IContractBusReplacementBusiness, ContractBusReplacementBusiness>();
        builder.Services.AddScoped<IContractDriverReplacementBusiness, ContractDriverReplacementBusiness>();
        builder.Services.AddScoped<ICustomerBusiness, CustomerBusiness>();
        builder.Services.AddScoped<ICustomerContractBusiness, CustomerContractBusiness>();
        builder.Services.AddScoped<IEmployeeBusiness, EmployeeBusiness>();
        builder.Services.AddScoped<ISettingPanelBusiness, SettingPanelBusiness>();
        builder.Services.AddScoped<IUserRegistrationQueueBusiness, UserRegistrationQueueBusiness>();
        builder.Services.AddScoped<IInvoiceBusiness, InvoiceBusiness>();
        builder.Services.AddScoped<IInvoiceExpenseBusiness, InvoiceExpenseBusiness>();
        builder.Services.AddScoped<ITerminationBusiness, TerminationBusiness>();
        builder.Services.AddScoped<IWebhookBusiness, WebhookBusiness>();
        builder.Services.AddScoped<ISupplierBusiness, SupplierBusiness>();
        builder.Services.AddScoped<IFinancialBusiness, FinancialBusiness>();
        builder.Services.AddScoped<ISupportTicketBusiness, SupportTicketBusiness>();
    }
}
