using Azure.Core;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.Repositories.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;

namespace BusesControl.Services.v1;

public class TwoFaService(
    IUnitOfWork _unitOfWork,
    IEmailService _emailService,
    IUserRepository _userRepository,
    ITwoFaRepository _twoFaRepository,
    INotificationContext _notificationContext
) : ITwoFaService
{
    public async Task<bool> CreateAsync(string ipLocation, CreateTwoRequest request) 
    {
        var userRecord = await _userRepository.GetByEmailAsync(request.Email);
        if (userRecord is null) 
        {
            _notificationContext.SetNotification(
                title: NotificationTitle.BadRequest,
                details: Message.User.CredentialsInvalid,
                statusCode: StatusCodes.Status401Unauthorized
            );
            return false;
        }

        var record = new TwoFaModel 
        { 
            UserEmail = request.Email,
            ExpirationAt = DateTime.UtcNow.AddMinutes(5),
            CreatedAt = DateTime.UtcNow,
            IpLocation = ipLocation,
        };

        var codeExists = true;
        while (codeExists)
        {
            record.Code = Guid.NewGuid().ToString()[..8];
            codeExists = await _twoFaRepository.ExistsAsync(record.Code);
        }

        await _twoFaRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        _emailService.SendEmailTwoFaCode(record.UserEmail, record.Code);

        return true;
    }

    public async Task<TwoFaValidateCodeResponse> ValidateCodeAsync(TwoFaValidateCodeRequest request)
    {
        var record = await _twoFaRepository.GetByCodeAsync(request.Code);
        if (record is null || record.UserEmail != request.Email)
        {
            _notificationContext.SetNotification(
                title: NotificationTitle.BadRequest,
                details: Message.TwoFa.CodeInvalid,
                statusCode: StatusCodes.Status400BadRequest
            );
            return default!;
        }

        if (DateTime.UtcNow >= record.ExpirationAt)
        {
            _notificationContext.SetNotification(
                title: NotificationTitle.BadRequest,
                details: Message.TwoFa.CodeInvalid,
                statusCode: StatusCodes.Status400BadRequest
            );
            return default!;
        }

        record.Token = Guid.NewGuid().ToString()[..32];
        _twoFaRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return new TwoFaValidateCodeResponse 
        { 
            Token = record.Token,
        };
    }

    public async Task<bool> ValidateTokenAsync(TwoFaValidateTokenRequest request)
    {
        var record = await _twoFaRepository.GetByTokenAsync(request.Token);
        if (record is null || record.UserEmail != request.Email)
            return false;

        if (request.IpLocation != record.IpLocation)
            return false;

        if (DateTime.UtcNow > record.ExpirationAt)
            return false;

        record.Used = true;
        _twoFaRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> CheckForNew(string ipLocation, TwoFaCheckForNewRequest request)
    {
        var checkForNew = await _twoFaRepository.ExistsTokenValidAsync(request.Email, ipLocation);
        if (!checkForNew)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.TwoFa.CheckForNew
            );
            return false;
        }

        return true;
    }
}