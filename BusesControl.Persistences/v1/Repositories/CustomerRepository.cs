using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class CustomerRepository(
    AppDbContext context
) : ICustomerRepository
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

    public async Task<bool> CreateAsync(CustomerModel record)
    {
        await _context.Customers.AddAsync(record);
        return true;
    }

    public bool Update(CustomerModel record)
    {
        _context.Customers.Update(record);
        return true;
    }

    public async Task<bool> ExistsByEmailOrPhoneNumberOrCpfOrCnpjAsync(string email, string phoneNumber, string? cpf, string? cnpj, Guid? id = null)
    {
        return await _context.Customers.AnyAsync(x => (x.Email == email || x.PhoneNumber == phoneNumber || (!string.IsNullOrEmpty(cpf) && x.Cpf == cpf) || (!string.IsNullOrEmpty(cnpj) && x.Cnpj == cnpj)) && (id == null || x.Id != id));
    }
}
