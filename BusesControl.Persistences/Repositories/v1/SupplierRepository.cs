using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class SupplierRepository(
    AppDbContext context
) : GenericRepository<SupplierModel>(context), ISupplierRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<SupplierModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null)
    {
        var query = _context.Suppliers.AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Name.Contains(search) || x.Cnpj.Contains(search) || x.PhoneNumber.Contains(search) || x.Email.Contains(search));
        }

        if (page > 0 && pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        return await query.ToListAsync();
    }

    public async Task<int> CountBySearchAsync(string? search = null)
    {
        var query = _context.Suppliers.AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Name.Contains(search) || x.Cnpj.Contains(search) || x.PhoneNumber.Contains(search) || x.Email.Contains(search));
        }

        return await query.CountAsync();
    }

    public async Task<SupplierModel?> GetByIdAsync(Guid id)
    {
        return await _context.Suppliers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByEmailOrPhoneNumberOrCnpjAsync(string email, string phoneNumber, string cnpj, Guid? id = null)
    {
        return await _context.Suppliers.AnyAsync(x => x.Id != id && (x.Email == email || x.PhoneNumber == phoneNumber || x.Cnpj == cnpj));
    }
}
