using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class EmployeeRepository(
    AppDbContext context
) : GenericRepository<EmployeeModel>(context), IEmployeeRepository
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

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Employees.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByEmailOrPhoneNumberOrCpfAsync(string email, string phoneNumber, string cpf, Guid? id = null)
    {
        return await _context.Employees.AnyAsync(x => (x.Email == email || x.PhoneNumber == phoneNumber || x.Cpf == cpf) && x.Id != id);
    }
}
