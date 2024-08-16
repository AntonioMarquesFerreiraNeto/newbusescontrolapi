using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class UserRegistrationQueueRepository(
    AppDbContext context
) : GenericRepository<UserRegistrationQueueModel>(context), IUserRegistrationQueueRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<UserRegistrationQueueModel>> FindAsync(int page, int pageSize, string? search = null)
    {
        var query = _context.UsersRegistrationQueue.Include(x => x.Employee).Include(x => x.Requester).Include(x => x.Approved).AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Employee.Name.Contains(search));
        }

        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        var records = await query.ToListAsync();

        return records;
    }

    public async Task<int> CountAsync(string? search = null)
    {
        var query = _context.UsersRegistrationQueue.Include(x => x.Employee).AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Employee.Name.Contains(search));
        }

        return await query.CountAsync();
    }

    public async Task<UserRegistrationQueueModel?> GetByIdAsync(Guid id)
    {
        return await _context.UsersRegistrationQueue.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<UserRegistrationQueueModel?> GetByUserWithEmployeeAsync(Guid userId)
    {
        return await _context.UsersRegistrationQueue.AsNoTracking().Include(x => x.Employee).SingleOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<UserRegistrationQueueModel?> GetByEmployeeAttributesAsync(string email, string cpf, DateOnly birthDate)
    {
        return await _context.UsersRegistrationQueue.AsNoTracking().Include(x => x.Employee).SingleOrDefaultAsync(x => x.Employee.Email == email && x.Employee.Cpf == cpf && x.Employee.BirthDate == birthDate);
    }

    public async Task<bool> ExistsByEmployee(Guid employeeId)
    {
        return await _context.UsersRegistrationQueue.AnyAsync(x => x.EmployeeId == employeeId);
    }
}
