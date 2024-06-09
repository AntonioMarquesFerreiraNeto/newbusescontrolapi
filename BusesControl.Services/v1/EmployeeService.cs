using AutoMapper;
using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Entities.Request;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using System.Text.RegularExpressions;

namespace BusesControl.Services.v1;

public class EmployeeService(
    IMapper _mapper,
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IUserService _userService,
    IEmployeeBusiness _employeeBusiness,
    IEmployeeRepository _employeeRepository
) : IEmployeeService
{
    private async Task<bool> CreateUserForEmployee(EmployeeModel employee)
    {
        if (employee.Type == EmployeeTypeEnum.Driver)
        {
            return false;
        }

        await _userService.CreateForEmployeeAsync(_mapper.Map<UserCreateRequest>(employee));
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        return true;
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

        _unitOfWork.BeginTransaction();

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

        await CreateUserForEmployee(record);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        await _unitOfWork.CommitAsync(true);

        return true;
    }
}
