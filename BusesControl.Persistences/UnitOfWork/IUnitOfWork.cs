namespace BusesControl.Persistence.UnitOfWork;

public interface IUnitOfWork
{
    void BeginTransaction();
    Task CommitAsync(bool isFinishTransaction = false);
    void Rollback();
}
