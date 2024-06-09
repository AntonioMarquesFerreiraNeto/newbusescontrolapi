using BusesControl.Entities.Enums;
using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class UserRegistrationQueueRepository(
    AppDbContext _context
) : IUserRegistrationQueueRepository
{
    public async Task<IEnumerable<UserRegistrationQueueModel>> FindAsync(int pageSize, int pageNumber, string? search)
    {
        var query = _context.UsersRegistrationQueue.Include(x => x.Employee).AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Employee.Name.Contains(search));
        }

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        
        var records = await query.ToListAsync();

        return records;
    }

    public async Task<UserRegistrationQueueModel?> GetByIdAsync(Guid id)
    {
        return await _context.UsersRegistrationQueue.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<UserRegistrationQueueModel?> GetByUserAsync(Guid userId)
    {
        return await _context.UsersRegistrationQueue.SingleOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<UserRegistrationQueueModel?> GetByEmployeeAttributesAsync(string email, string cpf, DateOnly birthDate)
    {
        return await _context.UsersRegistrationQueue.Include(x => x.Employee).AsNoTracking().SingleOrDefaultAsync(x => x.Employee.Email == email && x.Employee.Cpf == cpf && x.Employee.BirthDate == birthDate);
    }

    public async Task<bool> CreateAsync(UserRegistrationQueueModel record)
    {
        await _context.UsersRegistrationQueue.AddAsync(record);
        return true;
    }

    public bool Update(UserRegistrationQueueModel record)
    {
        _context.UsersRegistrationQueue.Update(record);
        return true;
    }

    public bool Delete(UserRegistrationQueueModel record)
    {
        _context.UsersRegistrationQueue.Remove(record);
        return true;
    }

    public async Task<bool> ExistsByEmployee(Guid employeeId)
    {
        return await _context.UsersRegistrationQueue.AnyAsync(x => x.EmployeeId == employeeId);
    }
}
