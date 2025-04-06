using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BusesControl.Api.Utils
{
    [Authorize]
    public class SupportChatHub : Hub
    {
        public SupportChatHub(ILogger<SupportChatHub> logger)
        {
            _logger = logger;
        }

        private readonly ILogger _logger;

        public override async Task OnConnectedAsync()
        {
            var userAuth = UserAuth.Get(Context.User!);

            var httpContext = Context.GetHttpContext();
            var ticketId = httpContext?.Request.Query["ticketId"].ToString();
            if (!string.IsNullOrEmpty(ticketId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(ticketId, userAuth.Role));
                _logger.LogInformation($"User {userAuth.Id} connected to ticket {ticketId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userAuth = UserAuth.Get(Context.User!);

            var httpContext = Context.GetHttpContext();
            var ticketId = httpContext?.Request.Query["ticketId"].ToString();
            if (!string.IsNullOrEmpty(ticketId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(ticketId, userAuth.Role));
                _logger.LogInformation($"User {userAuth.Id} disconnected from ticket {ticketId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        private static string GetGroupName(string ticketId, string role)
        {
            return role switch
            {
                "SupportAgent" => $"{KeysSocket.SupportAgentChatHub}-${ticketId}",
                _ => $"{KeysSocket.SupportUserChatHub}-${ticketId}"
            };
        }
    }
}
