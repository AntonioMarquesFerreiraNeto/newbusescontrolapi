﻿using BusesControl.Entities.Models.v1;
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

    public async Task<IEnumerable<UserModel>> FindBySearchAsync(int page, int pageSize, string? search = null)
    {
        var query = _context.Users.Include(x => x.Employee).Where(x => x.EmployeeId.HasValue).AsNoTracking();

        if (search is not null)
        {
            query = query.Where(x => x.Employee!.Name.Contains(search));
        }

        query = query.Skip((page - 1) * pageSize).Take(pageSize);    

        return await query.ToListAsync();
    }

    public async Task<int> CountBySearchAsync(string? search = null)
    {
        return search is not null ? await _context.Users.CountAsync(x => x.Employee!.Name.Contains(search)) : await _context.Users.CountAsync();
    }

    public async Task<UserModel?> GetByEmailAndCpfAndBirthDateAsync(string email, string cpf, DateOnly birthDate)
    {
        Expression<Func<UserModel, bool>> criteria = x => x.Email == email &&
                                                          x.Employee!.Cpf == cpf &&
                                                          x.Employee.BirthDate == birthDate;

        return await _context.Users.AsNoTracking().Include(x => x.Employee).SingleOrDefaultAsync(criteria);
    }

    public async Task<UserModel?> GetByIdWithEmployeeAsync(Guid id)
    {
        return await _context.Users.AsNoTracking().Include(x => x.Employee).SingleOrDefaultAsync(x => x.Id == id);
    }
}
