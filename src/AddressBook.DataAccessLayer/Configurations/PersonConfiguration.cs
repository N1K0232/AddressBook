using AddressBook.DataAccessLayer.Configurations.Common;
using AddressBook.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBook.DataAccessLayer.Configurations;

internal class PersonConfiguration : BaseEntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(p => p.FirstName).HasMaxLength(255).IsRequired();
        builder.Property(p => p.LastName).HasMaxLength(255).IsRequired();

        builder.HasOne(p => p.City)
            .WithMany(c => c.People)
            .HasForeignKey(p => p.CityId)
            .HasConstraintName("FK_People_Cities")
            .IsRequired();

        builder.ToTable("People");
        base.Configure(builder);
    }
}