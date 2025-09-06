using AddressBook.DataAccessLayer.Configurations.Common;
using AddressBook.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBook.DataAccessLayer.Configurations;

internal class CityConfiguration : BaseEntityConfiguration<City>
{
    public override void Configure(EntityTypeBuilder<City> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();

        builder.HasIndex(c => c.Name)
            .IsClustered(false)
            .IsUnique()
            .HasDatabaseName("IX_Cities_Name");

        builder.ToTable("Cities");
        base.Configure(builder);
    }
}