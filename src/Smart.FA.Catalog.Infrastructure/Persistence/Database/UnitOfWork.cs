using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Smart.FA.Catalog.Core.SeedWork;

namespace Smart.FA.Catalog.Infrastructure.Persistence.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly CatalogContext _catalogContext;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public void RegisterNew(object entity)
    {
        switch (entity)
        {
            case Entity domainEntity when !IsRegistered(domainEntity):
                _catalogContext.Add(domainEntity);
                return;

            case ValueObject domainValueObject when !IsRegistered(domainValueObject):
                _catalogContext.Add(domainValueObject);
                return;
        }
    }

    public void RegisterDirty(object entity)
    {
        if (entity is not Entity domainEntity) return;
        if (!IsRegistered(domainEntity)) return;

        if (_catalogContext.Entry(domainEntity).State != EntityState.Modified)
            _catalogContext.Update(domainEntity);
    }

    public void RegisterClean(object entity)
    {
        if (entity is not Entity domainEntity) return;
        if (!IsRegistered(domainEntity)) return;

        _catalogContext.Entry(domainEntity).State = EntityState.Unchanged;
    }

    public void RegisterDeleted(object entity)
    {
        switch (entity)
        {
            case Entity domainEntity when !IsRegistered(domainEntity):
                return;

            case Entity domainEntity:
                {
                    if (_catalogContext.Entry(domainEntity).State != EntityState.Deleted)
                        _catalogContext.Remove(domainEntity);

                    break;
                }
            case ValueObject domainValueObject when !IsRegistered(domainValueObject):
                return;

            case ValueObject domainValueObject:
                {
                    if (_catalogContext.Entry(domainValueObject).State != EntityState.Deleted)
                        _catalogContext.Remove(domainValueObject);

                    break;
                }
        }
    }

    public void Commit()
    {
        _catalogContext.SaveChanges();
    }

    public void Rollback()
    {
        var changedEntries = _catalogContext.ChangeTracker.Entries()
                                     .Where(x => x.State != EntityState.Unchanged).ToList();
        changedEntries.ForEach(e =>
       {
           switch (e.State)
           {
               case EntityState.Modified:
                   e.CurrentValues.SetValues(e.OriginalValues);
                   e.State = EntityState.Unchanged;
                   break;
               case EntityState.Added:
                   e.State = EntityState.Detached;
                   break;
               case EntityState.Deleted:
                   e.State = EntityState.Unchanged;
                   break;
           }
       });
    }

    public void BeginTransaction()
    {
        _transaction = _catalogContext.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        _transaction?.Commit();
        _transaction = null;
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();

        _transaction = null;
    }

    private bool IsRegistered(Entity entity)
    {
        return _catalogContext.ChangeTracker
                       .Entries()
                       .Any(ee => ee.Entity is Entity trackedEntity
                               && trackedEntity.GetType() == entity.GetType()
                               && trackedEntity == entity);
    }

    private bool IsRegistered(ValueObject valueObject)
    {
        return _catalogContext.ChangeTracker
                       .Entries()
                       .Any(ee => ee.Entity is ValueObject trackedValueObject
                               && trackedValueObject.GetType() == valueObject.GetType()
                               && trackedValueObject == valueObject);
    }
}
