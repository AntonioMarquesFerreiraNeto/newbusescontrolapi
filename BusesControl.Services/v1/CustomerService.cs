using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class CustomerService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    ICustomerBusiness _customerBusiness,
    ICustomerRepository _customerRepository
) : ICustomerService
{
    public async Task<IEnumerable<CustomerModel>> FindBySearchAsync(int page, int pageSize, string? search = null)
    {
        var records = await _customerRepository.FindBySearchAsync(pageSize, page, search);
        return records;
    }

    public async Task<CustomerModel> GetByIdAsync(Guid id)
    {
        var record = await _customerRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
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
        request.Cpf = request.Cpf is not null ? OnlyNumbers.ClearValue(request.Cpf) : null;
        request.Cnpj = request.Cnpj is not null ? OnlyNumbers.ClearValue(request.Cnpj) : null;
        request.BirthDate = (request.Type == CustomerTypeEnum.NaturalPerson) ? request.BirthDate : null;
        request.OpenDate = (request.Type == CustomerTypeEnum.LegalEntity) ? request.OpenDate : null;
        request.PhoneNumber = OnlyNumbers.ClearValue(request.PhoneNumber);

        await _customerBusiness.ExistsByEmailOrPhoneNumberOrCpfOrCnpjAsync(request.Email, request.PhoneNumber, request.Cpf, request.Cnpj);
        if (_notificationApi.HasNotification)
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
           State = request.State,
           Type = request.Type
        };

        await _customerRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid id)
    {
        var record = await _customerRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
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
