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

public class SettingPanelService(
    IUnitOfWork _unitOfWork,
    IUserService _userService,
    ISettingPanelBusiness _settingPanelBusiness,
    INotificationApi _notificationApi,
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
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SettingPanel.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<IEnumerable<SettingPanelModel>> FindAsync(int page, int pageSize)
    {
        var records = await _settingPanelRepository.FindAsync(page, pageSize);
        return records;
    }

    public async Task<bool> CreateAsync(SettingPanelCreateRequest request)
    {
        await _settingPanelBusiness.ExistsByParentAsync(request.Parent);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

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

        await _settingPanelRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, SettingPanelUpdateRequest request)
    {
        var record = await _settingPanelBusiness.GetForUpdateAsync(id, request.Parent);
        if (_notificationApi.HasNotification)
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
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        _settingPanelRepository.Delete(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
