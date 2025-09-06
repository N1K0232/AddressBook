using AddressBook.DataAccessLayer.Entities.Common;

namespace AddressBook.DataAccessLayer;

public interface IDataContext
{
    ValueTask<T> GetAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : BaseEntity;

    IQueryable<T> GetData<T>(bool trackingChanges = false) where T : BaseEntity;

    Task InsertAsync<T>(T entity, CancellationToken cancellationToken = default) where T : BaseEntity;
}