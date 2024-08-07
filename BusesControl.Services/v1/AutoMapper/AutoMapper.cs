﻿using AutoMapper;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using System.Text.Json;

namespace BusesControl.Services.v1.AutoMapper;

public class AutoMapper : Profile
{
    public AutoMapper() 
    {
        CreateMap<EmployeeModel, UserCreateRequest>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<UserModel, UserResponse>();
        CreateMap<CustomerCreateRequest, CustomerUpdateRequest>();
        CreateMap<CustomerUpdateRequest, CustomerCreateRequest>();

        CreateMap<ContractModel, ContractResponse>();
        CreateMap<CustomerModel, CustomerResponse>();
        CreateMap<CustomerContractModel, CustomerContractResponse>();
        CreateMap<EmployeeModel, EmployeeResponse>();
        CreateMap<BusModel, BusResponse>();
        CreateMap<SettingPanelModel, SettingPanelResponse>();
        CreateMap<ContractDescriptionModel, ContractDescriptionResponse>();

        CreateMap<WebhookModel, WebhookResponse>()
            .ForMember(dest => dest.Events, opt => opt.MapFrom(src => DeserializeEvents(src.Events)));

        CreateMap<FinancialModel, FinancialResponse>();
        CreateMap<InvoiceModel, InvoiceResponse>();
        CreateMap<InvoiceExpenseModel, InvoiceExpenseResponse>();
        CreateMap<SupplierModel, SupplierResponse>();

        CreateMap<SupportTicketModel, SupportTicketResponse>();
        CreateMap<SupportTicketMessageModel, SupportTicketMessageResponse>();
    }

    private static IEnumerable<string> DeserializeEvents(string events)
    {
        return JsonSerializer.Deserialize<IEnumerable<string>>(events) ?? [];
    }
}
