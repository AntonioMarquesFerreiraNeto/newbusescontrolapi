using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Request;
using BusesControl.Entities.Response;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class EmployeeService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IUserService _userService,
    IEmployeeBusiness _employeeBusiness,
    IEmployeeRepository _employeeRepository
) : IEmployeeService
{
    public async Task<IEnumerable<EmployeeModel>> FindBySearchAsync(int page, int pageSize, string? search = null)
    {
        var records = await _employeeRepository.FindBySearchAsync(page, pageSize, search);
        return records;
    }

    public async Task<EmployeeModel> GetByIdAsync(Guid id)
    {
        var record = await _employeeRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Employee.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<bool> CreateAsync(EmployeeCreateRequest request)
    {
        request.Cpf = OnlyNumbers.ClearValue(request.Cpf);
        request.PhoneNumber = OnlyNumbers.ClearValue(request.PhoneNumber);

        await _employeeBusiness.ExistsByEmailOrPhoneNumberOrCpfAsync(request.Email, request.PhoneNumber, request.Cpf);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        var record = new EmployeeModel
        {
            Name = request.Name,
            Cpf = request.Cpf,
            BirthDate = request.BirthDate,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            HomeNumber = request.HomeNumber,
            Logradouro = request.Logradouro,
            ComplementResidential = request.ComplementResidential,
            Neighborhood = request.Neighborhood,
            City = request.City,
            State = request.State,
            Type = request.Type
        };

        await _employeeRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, EmployeeUpdateRequest request)
    {
        request.Cpf = OnlyNumbers.ClearValue(request.Cpf);
        request.PhoneNumber = OnlyNumbers.ClearValue(request.PhoneNumber);

        var record = await _employeeRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Employee.NotFound
            );
            return false;
        }

        await _employeeBusiness.ExistsByEmailOrPhoneNumberOrCpfAsync(request.Email, request.PhoneNumber, request.PhoneNumber, id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        record.Name = request.Name;
        record.Cpf = request.Cpf;
        record.BirthDate = request.BirthDate;
        record.PhoneNumber = request.PhoneNumber;
        record.Email = request.Email;
        record.HomeNumber = request.HomeNumber;
        record.Logradouro = request.Logradouro;
        record.ComplementResidential = request.ComplementResidential;
        record.Neighborhood = request.Neighborhood;
        record.City = request.City;
        record.State = request.State;

        _employeeRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid id)
    {
        var record = await _employeeRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Employee.NotFound
            );
            return false;
        }

        record.Status = record.Status == EmployeeStatusEnum.Inactive ? EmployeeStatusEnum.Active : EmployeeStatusEnum.Inactive;
        _employeeRepository.Update(record);
        await _unitOfWork.CommitAsync();    

        return true;
    }

    public async Task<SuccessResponse> ToggleTypeAsync(Guid id, EmployeeToggleTypeRequest request)
    {
        var record = await _employeeBusiness.GetForToggleTypeAsync(id, request);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        record.Type = request.Type;
        _employeeRepository.Update(record);
        await _unitOfWork.CommitAsync();

        var message = await _userService.ToggleRoleForEmployeeAsync(record.Email, record.Type);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        await _unitOfWork.CommitAsync(true);

        return message;
    }
}
