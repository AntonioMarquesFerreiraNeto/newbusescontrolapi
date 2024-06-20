using AutoMapper;
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
using Microsoft.AspNetCore.Identity;

namespace BusesControl.Services.v1;

public class UserRegistrationQueueService(
    IMapper _mapper,
    IUnitOfWork _unitOfWork,
    INotificationApi _notificationApi,
    UserManager<UserModel> _userManager,
    IUserService _userService,
    IUserRegistrationQueueBusiness _userRegistrationQueueBusiness,
    IUserRegistrationQueueRepository _userRegistrationQueueRepository,
    IUserRegistrationSecurityCodeRepository _userRegistrationSecurityCodeRepository
) : IUserRegistrationQueueService
{
    private async Task<string> GenerateUniqueCode()
    {
        var random = new Random();
        var chars = "abcdefghijklnopqyz0123456789";
        var code = "";

        var existsCode = true;
        while (existsCode)
        {
            for (int c = 0; c < AppSettingsSecurityCode.CodeLenght; c++)
            {
                code += chars[random.Next(chars.Length)];
            }

            existsCode = await _userRegistrationSecurityCodeRepository.ExistsByCode(code);
        }

        return code;
    }

    public async Task<IEnumerable<UserRegistrationQueueModel>> FindBySearchAsync(int pageSize, int pageNumber, string? search)
    {
        var records = await _userRegistrationQueueRepository.FindAsync(pageSize, pageNumber, search);
        return records;
    }

    public async Task<SuccessResponse> CreateForEmployeeAsync(UserRegistrationCreateRequest request)
    {    
        await _userRegistrationQueueBusiness.ValidateForCreateAsync(request.EmployeeId);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        var record = new UserRegistrationQueueModel
        {
            RequestId = _userService.FindAuthenticatedUser().Id,
            EmployeeId = request.EmployeeId,
        };

        await _userRegistrationQueueRepository.CreateAsync(record);
        await _unitOfWork.CommitAsync();

        //TODO: Enviar e-mail com a rota do front para iniciar o processo de definição de senha no e-mail do funcionário. 

        return new SuccessResponse(Message.UserRegistration.SuccessCreate);
    }

    public async Task<SuccessResponse> RegistrationUserStepCodeAsync(UserRegistrationStepCodeRequest request)
    {
        request.Cpf = OnlyNumbers.ClearValue(request.Cpf);

        var record = await _userRegistrationQueueBusiness.GetForRegistrationUserStepCodeAsync(request);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        var userRecord = await _userService.CreateForUserRegistrationAsync(_mapper.Map<UserCreateRequest>(record.Employee));
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        var securityCodeRecord = new UserRegistrationSecurityCodeModel
        {
            Code = await GenerateUniqueCode(),
            UserId = userRecord.Id,
            Expires = DateTime.UtcNow.AddMinutes(AppSettingsSecurityCode.ExpireCode)    
        };
        await _userRegistrationSecurityCodeRepository.CreateAsync(securityCodeRecord);
        await _unitOfWork.CommitAsync();

        record.Status = UserRegistrationQueueStatusEnum.WaitingForPassword;
        record.UpdatedAt = DateTime.UtcNow;
        record.UserId = userRecord.Id;
        _userRegistrationQueueRepository.Update(record);

        //TODO: enviar código de definição de senha para o e-mail do usuário.

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.UserRegistration.SuccessStepCode);
    }

    public async Task<UserRegistrationStepTokenResponse> RegistrationUserStepTokenAsync(UserRegistrationStepTokenRequest request)
    {
        var record = await _userRegistrationSecurityCodeRepository.GetByCodeAsync(request.Code);
        if (record is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.UserRegistration.CodeNotFound
            );
            return default!;
        }

        var difference = DateTime.UtcNow - record.Expires;
        var expires = TimeSpan.FromMinutes(AppSettingsSecurityCode.ExpireCode);
        if (difference >= expires)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.UserRegistration.CodeInvalid
            );
            return default!;
        }

        var userRecord = await _userManager.FindByIdAsync(record.UserId.ToString());
        if (userRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        var tokenPassword = await _userManager.GeneratePasswordResetTokenAsync(userRecord);
        if (tokenPassword is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.UserRegistration.Unexpected
            );
            return default!;
        }

        return new UserRegistrationStepTokenResponse(userRecord.Id, tokenPassword);
    }

    public async Task<SuccessResponse> RegistrationUserStepPasswordAsync(UserRegistrationStepPasswordRequest request)
    {
        if (request.NewPassword != request.ConfirmPassword)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ResetUser.InvalidPassword
            );
            return default!;
        }

        var userRegistrationQueueRecord = await _userRegistrationQueueBusiness.GetForRegistrationUserStepPasswordAsync(request.UserId);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        var userRecord = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (userRecord is null)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        _unitOfWork.BeginTransaction();

        var result = await _userManager.ResetPasswordAsync(userRecord, request.ResetToken, request.NewPassword);
        if (!result.Succeeded)
        {
            _notificationApi.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return default!;
        }

        userRegistrationQueueRecord.Status = UserRegistrationQueueStatusEnum.Finished;
        userRegistrationQueueRecord.UpdatedAt = DateTime.UtcNow;
        _userRegistrationQueueRepository.Update(userRegistrationQueueRecord);

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.UserRegistration.Success);
    }

    public async Task<SuccessResponse> DeleteAsync(Guid id)
    {
        var record = await _userRegistrationQueueBusiness.GetForDeleteAsync(id);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        if (record.UserId is not null)
        {
            await _userService.DeleteForUserRegistrationAsync(record.UserId.Value);
            if (_notificationApi.HasNotification)
            {
                return default!;
            }
        }

        _userRegistrationQueueRepository.Delete(record);

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.UserRegistration.SuccessCanceled);
    }

    public async Task<SuccessResponse> AprrovedAsync(Guid id)
    {
        var record = await _userRegistrationQueueBusiness.GetForApprovedAsync(id);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        await _userService.ActiveForAprrovedUserRegistrationAsync(record.UserId!.Value);
        if (_notificationApi.HasNotification)
        {
            return default!;
        }

        record.Status = UserRegistrationQueueStatusEnum.Approved;
        record.ApprovedId = _userService.FindAuthenticatedUser().Id;
        record.UpdatedAt = DateTime.UtcNow;
        _userRegistrationQueueRepository.Update(record);

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.UserRegistration.SuccessApproved);
    }
}
