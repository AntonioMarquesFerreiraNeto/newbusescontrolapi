using AutoMapper;
using BusesControl.Business.v1.Interfaces;
using BusesControl.Commons.Notification.Interfaces;
using BusesControl.Entities.Enums.v1;
using BusesControl.Entities.Models.v1;
using BusesControl.Entities.Requests.v1;
using BusesControl.Entities.Responses.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using BusesControl.Persistence.UnitOfWork;
using BusesControl.Services.v1.Interfaces;

namespace BusesControl.Services.v1;

public class SupportTicketMessageService(
    IMapper _mapper,
    INotificationContext _notificationContext,
    IUnitOfWork _unitOfWork,
    IUserService _userService,
    ISupportTicketBusiness _supportTicketBusiness,
    ISupportTicketRepository _supportTicketRepository,
    ISupportTicketMessageRepository _supportTicketMessageRepository
) : ISupportTicketMessageService
{
    public async Task<IEnumerable<SupportTicketMessageResponse>> FindByTicketAsync(Guid ticketId)
    {
        var authenticatedUser = _userService.FindAuthenticatedUser();
        var isSupportAgent = authenticatedUser.Role == "SupportAgent";

        var messages = await _supportTicketMessageRepository.FindByTicketAsync(ticketId);

        var messagesToMarkAsDelivered = messages.Where(message => message.IsSupportAgent != isSupportAgent).ToList();

        foreach (var message in messagesToMarkAsDelivered)
        {
            var shouldBeMarkedAsDelivered = !isSupportAgent;
            await _supportTicketMessageRepository.MarkMessageAsDeliveredAsync(message.Id, shouldBeMarkedAsDelivered);
        }

        return _mapper.Map<IEnumerable<SupportTicketMessageResponse>>(messages);
    }

    public async Task<bool> CreateInternalAsync(Guid ticketId, string message)
    {
        var record = new SupportTicketMessageModel
        {
            SupportTicketId = ticketId,
            Message = message,
            IsSupportAgent = false
        };
        await _supportTicketMessageRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        return false;
    }

    public async Task<SupportTicketMessageResponse> CreateAsync(Guid ticketId, SupportTicketMessageCreateRequest request)
    {
        var user = _userService.FindAuthenticatedUser();
        var employeeId = user.Role != "SupportAgent" ? user.EmployeeId : null;
        var supportAgentId = user.Role == "SupportAgent" ? user.EmployeeId : null;

        var supportTicketRecord = await _supportTicketBusiness.GetForCreateSupportTicketMessageAsync(ticketId, employeeId);
        if (_notificationContext.HasNotification)
        {
            return default!;
        }

        _unitOfWork.BeginTransaction();

        var record = new SupportTicketMessageModel
        {
            Message = request.Message,
            SupportTicketId = ticketId,
            SupportAgentId = supportAgentId,
            IsSupportAgent = supportAgentId is not null,
        };
        await _supportTicketMessageRepository.AddAsync(record);
        await _unitOfWork.CommitAsync();

        if (supportAgentId is not null && supportTicketRecord.Status != SupportTicketStatusEnum.InProgress)
        {
            supportTicketRecord.Status = SupportTicketStatusEnum.InProgress;
            supportTicketRecord.UpdateAt = DateTime.UtcNow;
            supportTicketRecord.SupportAgentId = supportAgentId;
            _supportTicketRepository.Update(supportTicketRecord);
            await _unitOfWork.CommitAsync();
        }

        await _unitOfWork.CommitAsync(true);

        return _mapper.Map<SupportTicketMessageResponse>(record);
    }
}
