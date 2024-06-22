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

    public async Task<IEnumerable<CustomerModel>> FindBySearchAsync(int pageSize = 0, int pageNumber = 0, string? search = null)
    {
        var query = _context.Customers.AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Name.Contains(search) || x.Email.Contains(search));
        }

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

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
        var query = _context.Customers.Where(x => (x.Email == email || x.PhoneNumber == phoneNumber) && x.Id != id);

        if (!string.IsNullOrEmpty(cpf))
        {
            query = query.Where(x => x.Cpf == cpf);
        }

        if (!string.IsNullOrEmpty(cnpj))
        {
            query = query.Where(x => x.Cnpj == cnpj);
        }

        return await query.AnyAsync();
    }
}
