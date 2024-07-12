using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class SavedCardRepository(
    AppDbContext context
) : ISavedCardRepository
{
    private readonly AppDbContext _context = context;

    public async Task<bool> CreateAsync(SavedCardModel record)
    {
        await _context.AddAsync(record);
        return true;
    }

    public async Task<SavedCardModel?> GetByCustomerAsync(Guid customerId)
    {
        return await _context.SavedCards.FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }
}
