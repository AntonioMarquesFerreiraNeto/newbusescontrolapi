using BusesControl.Persistence.Contexts;

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
    }
}
