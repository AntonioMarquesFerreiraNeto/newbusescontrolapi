using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class EmployeeRepository(
    AppDbContext context
) : IEmployeeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<EmployeeModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null)
    {
        var query = _context.Employees.AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Name.Contains(search) || x.Cpf.Contains(search) || x.Email.Contains(search));
        }

        query = query.Skip((page - 1) * pageSize).Take(pageSize);
        
        var records = await query.ToListAsync();

        return records;
    }

    public async Task<EmployeeModel?> GetByIdAsync(Guid id)
    {
        return await _context.Employees.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CreateAsync(EmployeeModel record)
    {
        await _context.Employees.AddAsync(record);
        return true;
    }

    public bool Update(EmployeeModel record)
    {
        _context.Employees.Update(record);
        return true;
    }

    public async Task<bool> ExistsByEmailOrPhoneNumberOrCpfAsync(string email, string phoneNumber, string cpf, Guid? id = null)
    {
        return await _context.Employees.AnyAsync(x => (x.Email == email ||x.PhoneNumber == phoneNumber || x.Cpf == cpf) && x.Id != id);
    }
}
