namespace BusesControl.Persistence.v1.UnitOfWork;

public interface IUnitOfWork
{
    void BeginTransaction();
    Task CommitAsync(bool isFinishTransaction = false);
    void Rollback();
}
