using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.Repositories;

public class EmployeeRepository(
    AppDbContext _context
) : IEmployeeRepository
{
    public async Task<EmployeeModel?> GetByIdAsync(Guid id)
    {
        return await _context.Employees.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> CreateAsync(EmployeeModel record)
    {
        await _context.AddAsync(record);
        return true;
    }

    public bool Update(EmployeeModel record)
    {
        _context.Update(record);
        return true;
    }

    public async Task<bool> ExistsByEmailOrPhoneNumberOrCpfAsync(string email, string phoneNumber, string cpf, Guid? id = null)
    {
        return await _context.Employees.AnyAsync(x =>
            (x.Email == email ||
            x.PhoneNumber == phoneNumber ||
            x.Cpf == cpf) && x.Id != id
        );
    }
}
