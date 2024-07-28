using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;

namespace BusesControl.Persistence.v1.Repositories;

public class SupportTicketMessageRepository(
    AppDbContext context
) : ISupportTicketMessageRepository
{
    private readonly AppDbContext _context = context;

    public async Task<bool> CreateAsync(SupportTicketMessageModel record)
    {
        await _context.SupportTicketMessages.AddAsync(record);
        return true;
    }
}
