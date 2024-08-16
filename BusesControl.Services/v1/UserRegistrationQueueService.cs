using AutoMapper;
using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons;
using BusesControl.Commons.Notification;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Filters.Notification;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BusesControl.Services.v1;

public class UserRegistrationQueueService(
    AppSettings _appSettings,
    UserManager<UserModel> _userManager,
    IMapper _mapper,
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    IEmailService _emailService,
    INotificationService _notificationService,
    IUserService _userService,
    IUserRegistrationQueueBusiness _userRegistrationQueueBusiness,
    IUserRegistrationQueueRepository _userRegistrationQueueRepository,
    IUserRegistrationSecurityCodeRepository _userRegistrationSecurityCodeRepository
) : IUserRegistrationQueueService
{
    private async Task<string> GenerateUniqueCode()
    {
        var random = new Random();
        var chars = "ABCDEFG0123456789";
        var code = "";

        var existsCode = true;
        while (existsCode)
        {
            for (int c = 0; c < _appSettings.SecurityCode.CodeLength; c++)
            {
                code += chars[random.Next(chars.Length)];
            }

            existsCode = await _userRegistrationSecurityCodeRepository.ExistsByCode(code);
        }

        return code;
    }

    public async Task<PaginationResponse<UserRegistrationQueueModel>> FindBySearchAsync(PaginationRequest request)
    {
        var records = await _userRegistrationQueueRepository.FindAsync(request.Page, request.PageSize, request.Search);
        var count = await _userRegistrationQueueRepository.CountAsync(request.Search);

        return new PaginationResponse<UserRegistrationQueueModel> 
        { 
            Response = records,
            TotalSize = count
        };
    }

    public async Task<SuccessResponse> CreateForEmployeeAsync(UserRegistrationCreateRequest request)
    {    
        var employeeRecord = await _userRegistrationQueueBusiness.GetForValidateForCreateAsync(request.EmployeeId);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        var record = new UserRegistrationQueueModel
        {
            RequesterId = _userService.FindAuthenticatedUser().EmployeeId!.Value,
            EmployeeId = request.EmployeeId,
        };

        await _userRegistrationQueueRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        _emailService.SendEmailForWelcomeUserRegistration(employeeRecord.Email, employeeRecord.Name);

        return new SuccessResponse(Message.UserRegistration.SuccessCreate);
    }

    public async Task<SuccessResponse> RegistrationUserStepCodeAsync(UserRegistrationStepCodeRequest request)
    {
        request.Cpf = OnlyNumbers.ClearValue(request.Cpf);

        var record = await _userRegistrationQueueBusiness.GetForRegistrationUserStepCodeAsync(request);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        var userRecord = await _userService.CreateForUserRegistrationAsync(_mapper.Map<UserCreateRequest>(record.Employee));
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        var securityCodeRecord = new UserRegistrationSecurityCodeModel
        {
            Code = await GenerateUniqueCode(),
            UserId = userRecord.Id,
            Expires = DateTime.UtcNow.AddMinutes(_appSettings.SecurityCode.ExpireCode)    
        };
        await _userRegistrationSecurityCodeRepository.AddAsync(securityCodeRecord);
        await _unitOfWork.CommitAsync();

        record.Status = UserRegistrationQueueStatusEnum.WaitingForPassword;
        record.UpdatedAt = DateTime.UtcNow;
        record.UserId = userRecord.Id;
        _userRegistrationQueueRepository.Update(record);

        _emailService.SendEmailStepCode(record.Employee.Email, record.Employee.Name, securityCodeRecord.Code);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.UserRegistration.SuccessStepCode);
    }

    public async Task<UserRegistrationStepTokenResponse> RegistrationUserStepTokenAsync(UserRegistrationStepTokenRequest request)
    {
        var record = await _userRegistrationSecurityCodeRepository.GetByCodeAsync(request.Code);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.UserRegistration.CodeNotFound
            );
            return default!;
        }

        var difference = DateTime.UtcNow - record.Expires;
        var expires = TimeSpan.FromMinutes(_appSettings.SecurityCode.ExpireCode);
        if (difference >= expires)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.UserRegistration.CodeInvalid
            );
            return default!;
        }

        var userRecord = await _userManager.FindByIdAsync(record.UserId.ToString());
        if (userRecord is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.User.NotFound
            );
            return default!;
        }

        var tokenPassword = await _userManager.GeneratePasswordResetTokenAsync(userRecord);
        if (tokenPassword is null)
        {
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status400BadRequest,
                title: NotificationTitle.BadRequest,
                details: Message.ResetUser.InvalidPassword
            );
            return default!;
        }

        var userRegistrationQueueRecord = await _userRegistrationQueueBusiness.GetForRegistrationUserStepPasswordAsync(request.UserId);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        var userRecord = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (userRecord is null)
        {
            _notificationContext.SetNotification(
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
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status500InternalServerError,
                title: NotificationTitle.InternalError,
                details: Message.User.Unexpected
            );
            return default!;
        }

        userRegistrationQueueRecord.Status = UserRegistrationQueueStatusEnum.Finished;
        userRegistrationQueueRecord.UpdatedAt = DateTime.UtcNow;
        _userRegistrationQueueRepository.Update(userRegistrationQueueRecord);

        await _notificationService.SendInternalNotificationAsync(
            TemplateTitle.UserRegistrationQueueReview, 
            TemplateMessage.UserRegistrationQueueReview(userRegistrationQueueRecord.Employee.Name), 
            NotificationAccessLevelEnum.Admin
        );

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.UserRegistration.Success);
    }

    public async Task<SuccessResponse> DeleteAsync(Guid id)
    {
        var record = await _userRegistrationQueueBusiness.GetForDeleteAsync(id);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        if (record.UserId is not null)
        {
            await _userService.DeleteForUserRegistrationAsync(record.UserId.Value);
            if (_notificationContext.HasNotification)
            {
                return default!;
            }
        }

        _userRegistrationQueueRepository.Remove(record);

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.UserRegistration.SuccessDelete);
    }

    public async Task<SuccessResponse> AprroveAsync(Guid id)
    {
        var record = await _userRegistrationQueueBusiness.GetForApproveAsync(id);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        await _userService.ActiveForAprrovedUserRegistrationAsync(record.UserId!.Value);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        record.Status = UserRegistrationQueueStatusEnum.Approved;
        record.ApprovedId = _userService.FindAuthenticatedUser().EmployeeId!.Value;
        record.UpdatedAt = DateTime.UtcNow;
        _userRegistrationQueueRepository.Update(record);

        await _unitOfWork.CommitAsync(true);

        return new SuccessResponse(Message.UserRegistration.SuccessApproved);
    }
}
