using AutoMapper;
using BusesControl.Business.v1.Interfaces;
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

public class SupportTicketService(
    IMapper _mapper,
    IUnitOfWork _unitOfWork,
    INotificationContext _notificationContext,
    IUserService _userService,
    ISupportTicketMessageService _supportTicketMessageService,
    ISupportTicketBusiness _supportTicketBusiness,
    ISupportTicketRepository _supportTicketRepository
) : ISupportTicketService
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
            existsReference = await _supportTicketRepository.ExistsByReferenceAsync(reference);
        }

        return reference;
    }

    public async Task<IEnumerable<SupportTicketModel>> FindByStatusAsync(PaginationRequest request, SupportTicketStatusEnum? status = null)
    {
        var user = _userService.FindAuthenticatedUser();
        var employeeId = (user.Role != "SupportAgent") ? user.EmployeeId : null;

        var records = await _supportTicketRepository.FindByStatusAsync(employeeId, status, request.Page, request.PageSize);
        
        return records;
    }

    public async Task<SupportTicketResponse> GetByIdAsync(Guid id)
    {
        var user = _userService.FindAuthenticatedUser();
        var employeeId = (user.Role != "SupportAgent") ? user.EmployeeId : null;

        var record = await _supportTicketRepository.GetByIdOptionalEmployeeWithIncludesAsync(id, employeeId);
        if (record is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.SupportTicket.NotFound
            );
            return default!;
        }

        record.SupportTicketMessages = [.. record.SupportTicketMessages.OrderByDescending(x => x.CreatedAt)];

        return _mapper.Map<SupportTicketResponse>(record);
    }

    public async Task<bool> CreateAsync(SupportTicketCreateRequest request)
    {
        var user = _userService.FindAuthenticatedUser();
        if (user.EmployeeId is null)
        {
            _notificationContext.SetNotification(
                statusCode: StatusCodes.Status404NotFound,
                title: NotificationTitle.NotFound,
                details: Message.Employee.NotFound
            );
            return false;
        }

        _unitOfWork.BeginTransaction();

        var record = new SupportTicketModel
        {
            Reference = await GenerateReferenceUniqueAsync(),
            EmployeeId = user.EmployeeId.Value,
            Title = request.Title,
            Type = request.Type
        };
        await _supportTicketRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        await _supportTicketMessageService.CreateInternalAsync(record.Id, request.Message);

        await _unitOfWork.CommitAsync(true);

        return true;
    }

    public async Task<bool> CloseAsync(Guid id)
    {
        var record = await _supportTicketBusiness.GetForCancelOrCloseAsync(id);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        record.Status = SupportTicketStatusEnum.Closed;
        record.UpdateAt = DateTime.UtcNow;
        record.SupportAgentId = _userService.FindAuthenticatedUser().EmployeeId;
        _supportTicketRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> CancelAsync(Guid id)
    {
        var record = await _supportTicketBusiness.GetForCancelOrCloseAsync(id);
        if (_notificationContext.HasNotification)
        {
            return false;
        }

        record.Status = SupportTicketStatusEnum.Canceled;
        record.UpdateAt = DateTime.UtcNow;
        record.SupportAgentId = _userService.FindAuthenticatedUser().EmployeeId;
        _supportTicketRepository.Update(record);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
