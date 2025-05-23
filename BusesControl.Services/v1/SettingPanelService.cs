﻿using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class SettingPanelService(
    IUnitOfWork _unitOfWork,
    IUserService _userService,
    ISettingPanelBusiness _settingPanelBusiness,
    INotificationContext _notificationContext,
    ISettingPanelRepository _settingPanelRepository
) : ISettingPanelService
{
    private async Task<string> GenerateReferenceUniqueAsync()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var reference = "#";
        var random = new Random();
        var existsReference = true;

        while (existsReference)
        {
            for (int c = 0; c < 7; c++)
            {
                reference += chars[random.Next(chars.Length)];
            }
            existsReference = await _settingPanelRepository.ExitsByReferenceAsync(reference);
        }

        return reference;
    }

    public async Task<SettingPanelModel> GetByIdAsync(Guid id)
    {
        var record = await _settingPanelRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingPanel.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<PaginationResponse<SettingPanelModel>> FindAsync(PaginationRequest request)
    {
        var records = await _settingPanelRepository.FindAsync(request.Page, request.PageSize);
        var count = await _settingPanelRepository.CountAsync();

        return new PaginationResponse<SettingPanelModel> 
        { 
            Response = records,
            TotalSize = count
        };
    }

    public async Task<IEnumerable<SettingPanelModel>> FindByParentAsync(SettingPanelParentEnum parent)
    {
        return await _settingPanelRepository.FindByParentAysnc(parent);
    }

    public async Task<bool> CreateAsync(SettingPanelCreateRequest request)
    {
        var employeeId = _userService.FindAuthenticatedUser().EmployeeId!.Value;
        var reference = await GenerateReferenceUniqueAsync();

        var record = new SettingPanelModel
        {
            Reference = reference,
            RequesterId = employeeId,
            TerminationFee = request.TerminationFee,
            LateFeeInterestRate = request.LateFeeInterestRate,
            CustomerDelinquencyEnabled = request.CustomerDelinquencyEnabled,
            LimitDateTerminate = request.LimitDateTerminate,
            Parent = request.Parent,
            Active = request.Active
        };

        await _settingPanelRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, SettingPanelUpdateRequest request)
    {
        var record = await _settingPanelBusiness.GetForUpdateAsync(id, request.Parent);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        var employeeId = _userService.FindAuthenticatedUser().EmployeeId!.Value;

        record.RequesterId = employeeId;
        record.TerminationFee = request.TerminationFee;
        record.LateFeeInterestRate = request.LateFeeInterestRate;
        record.CustomerDelinquencyEnabled = request.CustomerDelinquencyEnabled;
        record.LimitDateTerminate = request.LimitDateTerminate;
        record.Parent = request.Parent;
        record.Active = request.Active;
        record.UpdatedAt = DateTime.UtcNow;
        
        _settingPanelRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var record = await _settingPanelBusiness.GetForDeleteAsync(id);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        _settingPanelRepository.Remove(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
