using AddressBook.DataAccessLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBook.DataAccessLayer.Configurations.Common;

internal abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(e => e.LastModifiedAt).ValueGeneratedOnUpdate();
    }
}