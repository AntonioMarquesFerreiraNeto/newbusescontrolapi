using BusesControl.Entities.Models;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.v1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusesControl.Persistence.v1.Repositories;

public class UserRepository(
    AppDbContext _context
) : IUserRepository
{
    public async Task<UserModel?> GetByEmailAndCpfAndBirthDateAsync(string email, string cpf, DateOnly birthDate)
    {
        Expression<Func<UserModel, bool>> criteria = x => x.Email == email && 
                                                          x.Employee.Cpf == cpf && 
                                                          x.Employee.BirthDate == birthDate;
        
        return await _context.Users.AsNoTracking().Include(x => x.Employee).SingleOrDefaultAsync(criteria); 
    }
}
