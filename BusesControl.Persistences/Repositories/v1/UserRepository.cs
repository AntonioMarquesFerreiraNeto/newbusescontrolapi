using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Contexts;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusesControl.Persistence.Repositories.v1;

public class UserRepository(
    AppDbContext context
) : GenericRepository<UserModel>(context), IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task<UserModel?> GetByEmailAndCpfAndBirthDateAsync(string email, string cpf, DateOnly birthDate)
    {
        Expression<Func<UserModel, bool>> criteria = x => x.Email == email &&
                                                          x.Employee!.Cpf == cpf &&
                                                          x.Employee.BirthDate == birthDate;

        return await _context.Users.AsNoTracking().Include(x => x.Employee).SingleOrDefaultAsync(criteria);
    }
}
