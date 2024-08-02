using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;

namespace BusesControl.Persistence.Repositories.v1;

public class SupportTicketMessageRepository(
    AppDbContext context
) : GenericRepository<SupportTicketMessageModel>(context), ISupportTicketMessageRepository
{
}
