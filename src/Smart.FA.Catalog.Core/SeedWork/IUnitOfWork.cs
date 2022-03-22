namespace Smart.FA.Catalog.Core.SeedWork;

public interface IUnitOfWork
{
    void RegisterNew(object entity);
    void RegisterDirty(object entity);
    void RegisterClean(object entity);
    void RegisterDeleted(object entity);
    void Commit();
    void Rollback();
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}
