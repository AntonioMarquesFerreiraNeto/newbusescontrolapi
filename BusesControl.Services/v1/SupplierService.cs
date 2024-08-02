using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class SupplierService(
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    ISupplierRepository _supplierRepository,
    ISupplierBusiness _supplierBusiness
) : ISupplierService
{
    private static (string cnpj, string phoneNumber) ClearValues(string cnpj, string phoneNumber)
    {
        cnpj = OnlyNumbers.ClearValue(cnpj);
        phoneNumber = OnlyNumbers.ClearValue(phoneNumber);

        return (cnpj, phoneNumber);
    }

    public async Task<IEnumerable<SupplierModel>> FindBySearchAsync(PaginationRequest request)
    {
        var records = await _supplierRepository.FindBySearchAsync(request.Page, request.PageSize, request.Search);
        return records;
    }

    public async Task<SupplierModel> GetByIdAsync(Guid id)
    {
        var record = await _supplierRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Supplier.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> CreateAsync(SupplierCreateRequest request)
    {
        var (cnpj, phoneNumber) = ClearValues(request.Cnpj, request.PhoneNumber);

        await _supplierBusiness.ExistsByEmailOrPhoneNumberOrCnpjAsync(request.Email, phoneNumber, cnpj);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        var record = new SupplierModel
        {
            Name = request.Name,
            Cnpj = cnpj,
            OpenDate = request.OpenDate,
            Email = request.Email,
            PhoneNumber = phoneNumber,
            HomeNumber = request.HomeNumber,
            Logradouro = request.Logradouro,
            ComplementResidential = request.ComplementResidential,
            Neighborhood = request.Neighborhood,
            City = request.City,
            State = request.State,
        };
        await _supplierRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, SupplierUpdateRequest request)
    {
        var (cnpj, phoneNumber) = ClearValues(request.Cnpj, request.PhoneNumber);

        var record = await _supplierRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Supplier.NotFound
            );
            return false;
        }

        await _supplierBusiness.ExistsByEmailOrPhoneNumberOrCnpjAsync(request.Email, phoneNumber, cnpj, id);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        record.Name = request.Name;
        record.Cnpj = cnpj;
        record.OpenDate = request.OpenDate;
        record.Email = request.Email;
        record.PhoneNumber = phoneNumber;
        record.HomeNumber = request.HomeNumber;
        record.Logradouro = request.Logradouro;
        record.ComplementResidential = request.ComplementResidential;
        record.Neighborhood = request.Neighborhood;
        record.City = request.City;
        record.State = request.State;
        _supplierRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid id)
    {
        var record = await _supplierRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Supplier.NotFound
            );
            return false;
        }

        record.Active = !record.Active;
        _supplierRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
