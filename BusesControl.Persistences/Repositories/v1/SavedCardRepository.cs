using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class SavedCardRepository(
    AppDbContext context
) : GenericRepository<SavedCardModel>(context), ISavedCardRepository 
{
    private readonly AppDbContext _context = context;

    public async Task<SavedCardModel?> GetByCustomerAsync(Guid customerId)
    {
        return await _context.SavedCards.FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }
}
