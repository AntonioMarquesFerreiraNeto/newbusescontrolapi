using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;

namespace BusesControl.Persistence.v1.Repositories;

public class CustomerRepository(
    AppDbContext context
) : ICustomerRepository
{
    private readonly AppDbContext _context = context;
}
