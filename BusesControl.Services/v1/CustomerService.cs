using AutoMapper;
using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.DTOs;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace BusesControl.Services.v1;

public class CustomerService(
    AppSettings _appSettings,
    IMapper _mapper,
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    ICustomerBusiness _customerBusiness,
    ICustomerRepository _customerRepository
) : ICustomerService
{
    private static CustomerCreateRequest ClearCustomerRequest(CustomerCreateRequest request)
    {
        request.Cpf = (request.Type == CustomerTypeEnum.NaturalPerson) ? OnlyNumbers.ClearValue(request.Cpf!) : null;
        request.Cnpj = (request.Type == CustomerTypeEnum.LegalEntity) ? OnlyNumbers.ClearValue(request.Cnpj!) : null;
        request.BirthDate = (request.Type == CustomerTypeEnum.NaturalPerson) ? request.BirthDate : null;
        request.Gender = (request.Type == CustomerTypeEnum.NaturalPerson) ? request.Gender : null;
        request.OpenDate = (request.Type == CustomerTypeEnum.LegalEntity) ? request.OpenDate : null;
        request.PhoneNumber = OnlyNumbers.ClearValue(request.PhoneNumber);

        return request;
    }

    private async Task<string> CreateInAssasAsync(CustomerModel customer)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var createCustomerInAssas = new
        {
            name = customer.Name,
            cpfCnpj = customer.Cpf ?? customer.Cnpj,
            mobilePhone = customer.PhoneNumber,
            email = customer.Email
        };

        var httpResult = await httpClient.PostAsJsonAsync($"{_appSettings.Assas.Url}/customers", createCustomerInAssas);
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Customer.Unexpected
            );
            return default!;
        }

        var customerExternal = await httpResult.Content.ReadFromJsonAsync<CreateCustomerInAssasDTO>();
        if (customerExternal is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Customer.Unexpected
            );
            return default!;
        }

        return customerExternal.Id;
    }

    private async Task<bool> UpdateInAssasAsync(string externalId, CustomerUpdateRequest customer)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("access_token", _appSettings.Assas.Key);

        var updateCustomerInAssas = new 
        {
            name = customer.Name,
            cpfCnpj = customer.Cpf ?? customer.Cnpj,
            mobilePhone = customer.PhoneNumber,
            email = customer.Email
        };

        var httpResult = await httpClient.PutAsJsonAsync($"{_appSettings.Assas.Url}/customers/{externalId}", updateCustomerInAssas);
        if (!httpResult.IsSuccessStatusCode)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.Customer.Unexpected
            );
            return false;
        }

        return true;
    }

    public async Task<PaginationResponse<CustomerModel>> FindBySearchAsync(PaginationRequest request)
    {
        var records = await _customerRepository.FindBySearchAsync(request.Page, request.PageSize, request.Search);
        var count = await _customerRepository.CountBySearchAsync(request.Search);
        
        return new PaginationResponse<CustomerModel> 
        { 
            Response = records,
            TotalSize = count
        };
    }

    public async Task<CustomerModel> GetByIdAsync(Guid id)
    {
        var record = await _customerRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Customer.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> CreateAsync(CustomerCreateRequest request)
    {
        ClearCustomerRequest(request);

        await _customerBusiness.ExistsByRequestAsync(request);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        var record = new CustomerModel
        {
            Name = request.Name,
            Cpf = request.Cpf,
            BirthDate = request.BirthDate,
            OpenDate = request.OpenDate,
            Cnpj = request.Cnpj,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            HomeNumber = request.HomeNumber,
            Logradouro = request.Logradouro,
            ComplementResidential = request.ComplementResidential,
            Neighborhood = request.Neighborhood,
            City = request.City,
            State = request.State.ToUpper(),
            Type = request.Type,
            Gender = request.Gender
        };

        var externalId = await CreateInAssasAsync(record);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        record.ExternalId = externalId;
        await _customerRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, CustomerUpdateRequest request)
    {
        var clearRequest = ClearCustomerRequest(_mapper.Map<CustomerCreateRequest>(request));
        request = _mapper.Map<CustomerUpdateRequest>(clearRequest);

        var record = await _customerRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Customer.NotFound
            );
            return default!;
        }

        await _customerBusiness.ExistsByRequestAsync(_mapper.Map<CustomerCreateRequest>(request), id);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        await UpdateInAssasAsync(record.ExternalId, request);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        record.Name = request.Name;
        record.Cpf = request.Cpf;
        record.BirthDate = request.BirthDate;
        record.OpenDate = request.OpenDate;
        record.Cnpj = request.Cnpj;
        record.Email = request.Email;
        record.PhoneNumber = request.PhoneNumber;
        record.HomeNumber = request.HomeNumber;
        record.Logradouro = request.Logradouro;
        record.ComplementResidential = request.ComplementResidential;
        record.Neighborhood = request.Neighborhood;
        record.City = request.City;
        record.State = request.State.ToUpper();
        record.Type = request.Type;
        record.Gender = request.Gender;

        _customerRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid id)
    {
        var record = await _customerRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Customer.NotFound
            );
            return false;
        }

        record.Active = !record.Active;
        _customerRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
