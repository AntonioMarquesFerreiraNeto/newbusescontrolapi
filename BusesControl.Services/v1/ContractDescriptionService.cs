﻿using BusesControl.Business.v1.Interfaces;
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

public class ContractDescriptionService(
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    IContractDescriptionRepository _contractDescriptionRepository,
    IContractDescriptionBusiness _contractDescriptionBusiness
) : IContractDescriptionService
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
            existsReference = await _contractDescriptionRepository.ExitsByReferenceAsync(reference);
        }

        return reference;
    }

    public async Task<ContractDescriptionModel> GetByIdAsync(Guid id)
    {
        var record = await _contractDescriptionRepository.GetByIdAsync(id);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ContractDescription.NotFound
            );
            return default!;
        }

        return record;
    }

    public async Task<IEnumerable<ContractDescriptionModel>> FindAsync(int page, int pageSize)
    {
        var records = await _contractDescriptionRepository.FindAsync(page, pageSize);
        return records;
    }

    public async Task<bool> CreateAsync(ContractDescriptionCreateRequest request)
    {
        var reference = await GenerateReferenceUniqueAsync();

        var record = new ContractDescriptionModel
        {
            Reference = reference,
            Owner = request.Owner,
            GeneralProvisions = request.GeneralProvisions,
            Title = request.Title,
            SubTitle = request.SubTitle,
            Objective = request.Objective,
            Copyright = request.Copyright
        };

        await _contractDescriptionRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, ContractDescriptionUpdateRequest request)
    {
        var record = await _contractDescriptionBusiness.GetForUpdateAsync(id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        record.Owner = request.Owner;
        record.GeneralProvisions = request.GeneralProvisions;
        record.Title = request.Title;
        record.SubTitle = request.SubTitle;
        record.Objective = request.Objective;
        record.Copyright = request.Copyright;

        _contractDescriptionRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        var contractDescriptionExists = await _contractDescriptionRepository.ExitsAsync(id);
        if (!contractDescriptionExists)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.ContractDescription.NotFound
            );
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var record = await _contractDescriptionBusiness.GetForDeleteAsync(id);
        if (_notificationApi.HasNotification)
        {
            return false;
        }

        _contractDescriptionRepository.Delete(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
