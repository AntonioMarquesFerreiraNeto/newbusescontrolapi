using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Repositories.v1;

public class CustomerRepository(
    AppDbContext context
) : GenericRepository<CustomerModel>(context), ICustomerRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<CustomerModel>> FindBySearchAsync(int page = 0, int pageSize = 0, string? search = null)
    {
        var query = _context.Customers.AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Name.Contains(search) || x.Email.Contains(search));
        }

        if (page > 0 & pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        var records = await query.ToListAsync();

        return records;
    }

    public async Task<CustomerModel?> GetByIdAsync(Guid id)
    {
        return await _context.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<CustomerModel?> GetByExternalAsync(string externalId)
    {
        return await _context.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.ExternalId == externalId);
    }

    public async Task<bool> ExistsByEmailOrPhoneNumberOrCpfOrCnpjAsync(string email, string phoneNumber, string? cpf, string? cnpj, Guid? id = null)
    {
        return await _context.Customers.AnyAsync(x => (x.Email == email || x.PhoneNumber == phoneNumber || !string.IsNullOrEmpty(cpf) && x.Cpf == cpf || !string.IsNullOrEmpty(cnpj) && x.Cnpj == cnpj) && (id == null || x.Id != id));
    }
}
