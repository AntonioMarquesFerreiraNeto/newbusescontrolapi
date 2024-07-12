using BusesControl.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.v1.UnitOfWork;

public class UnitOfWork(
     AppDbContext _context
) : IUnitOfWork
{
    public void BeginTransaction()
    {
        _context.Database.BeginTransaction();
    }

    public async Task CommitAsync(bool isFinishTransaction = false)
    {
        await _context.SaveChangesAsync();
        if (isFinishTransaction)
        {
            _context.Database.CurrentTransaction?.Commit();
        }
    }

    public void Rollback()
    {
        _context.Database.CurrentTransaction?.Rollback();
        _context.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Detached);
    }
}
