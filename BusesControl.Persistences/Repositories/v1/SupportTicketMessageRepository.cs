using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace BusesControl.Persistence.Repositories.v1;

public class SupportTicketMessageRepository(
    AppDbContext context,
    IConfiguration _configuration,
    ILogger<SupportTicketMessageRepository> _logger
) : GenericRepository<SupportTicketMessageModel>(context), ISupportTicketMessageRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<SupportTicketMessageModel>> FindByTicketAsync(Guid ticketId)
    {
        return await _context.SupportTicketMessages.Where(x => x.SupportTicketId == ticketId).Include(x => x.SupportAgent).ToListAsync();
    }

    public async Task MarkMessageAsDeliveredAsync(Guid id, bool isSupportAgent)
    {
        try
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync();

            var execute = "UPDATE SupportTicketMessages SET Delivered = 1 WHERE Id = @MessageId AND IsSupportAgent = @IsSupportAgent";
            await connection.ExecuteAsync(execute, new { MessageId = id, IsSupportAgent = isSupportAgent });

            _logger.LogInformation("message marked as successfully delivered");
        }
        catch (Exception ex)
        {
            _logger.LogError("failed in mark message as delivered. Message Id : {MessageId}. Error Details: {ErrorDetails}", id, ex);
        }
    }
}
