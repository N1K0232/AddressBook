using System.Reflection;
using AddressBook.DataAccessLayer.Entities.Common;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.DataAccessLayer;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IDataContext
{
    public async ValueTask<T> GetAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : BaseEntity
    {
        var entity = await Set<T>().FindAsync([id], cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public IQueryable<T> GetData<T>(bool trackingChanges = false) where T : BaseEntity
    {
        var set = Set<T>();
        return trackingChanges ? set.AsTracking() : set.AsNoTrackingWithIdentityResolution();
    }

    public async Task InsertAsync<T>(T entity, CancellationToken cancellationToken = default) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        await Set<T>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await SaveChangesAsync(true, cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        entity.LastModifiedAt = DateTime.UtcNow;

        Set<T>().Update(entity);
        await SaveChangesAsync(true, cancellationToken).ConfigureAwait(false);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}