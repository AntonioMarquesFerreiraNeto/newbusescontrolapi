using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models;
using BusesControl.Entities.Requests;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using BusesControl.Persistence.v1.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class SettingsPanelService(
    IUnitOfWork _unitOfWork,
    IUserService _userService,
    ISettingsPanelBusiness _settingsPanelBusiness,
    INotificationApi _notificationApi,
    ISettingsPanelRepository _settingsPanelRepository
) : ISettingsPanelService
{
    public async Task<SettingsPanelModel> GetByIdAsync(Guid id)
    {
        var record = await _settingsPanelRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingsPanel.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<IEnumerable<SettingsPanelModel>> FindAsync(int page, int pageSize)
    {
        var records = await _settingsPanelRepository.FindAsync(page, pageSize);
        return records;
    }

    public async Task<bool> CreateAsync(SettingsPanelCreateRequest request)
    {
        await _settingsPanelBusiness.ExistsByParentAsync(request.Parent);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        var employeeId = _userService.FindAuthenticatedUser().EmployeeId;

        var record = new SettingsPanelModel
        {
            RequesterId = employeeId,
            TerminationFee = request.TerminationFee,
            LateFeeInterestRate = request.LateFeeInterestRate,
            CustomerDelinquencyEnabled = request.CustomerDelinquencyEnabled,
            LimitDateTermination = request.LimitDateTermination,
            Parent = request.Parent,
            Active = request.Active
        };

        await _settingsPanelRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, SettingsPanelUpdateRequest request)
    {
        var record = await _settingsPanelBusiness.GetForUpdateAsync(id, request.Parent);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        var employeeId = _userService.FindAuthenticatedUser().EmployeeId;

        record.RequesterId = employeeId;
        record.TerminationFee = request.TerminationFee;
        record.LateFeeInterestRate = request.LateFeeInterestRate;
        record.CustomerDelinquencyEnabled = request.CustomerDelinquencyEnabled;
        record.LimitDateTermination = request.LimitDateTermination;
        record.Parent = request.Parent;
        record.Active = request.Active;
        record.UpdatedAt = DateTime.UtcNow;
        
        _settingsPanelRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var record = await _settingsPanelRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingsPanel.NotFound
            );
            return false;
        }

        _settingsPanelRepository.Delete(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
