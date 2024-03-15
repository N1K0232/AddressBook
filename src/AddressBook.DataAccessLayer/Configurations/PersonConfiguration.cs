using AddressBook.DataAccessLayer.Configurations.Common;
using AddressBook.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBook.DataAccessLayer.Configurations;

public class PersonConfiguration : BaseEntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(p => p.FirstName).HasMaxLength(256).IsRequired();
        builder.Property(p => p.LastName).HasMaxLength(256).IsRequired();

        builder.Property(p => p.Gender).HasMaxLength(10).IsRequired().IsUnicode(false);
        builder.Property(p => p.City).HasMaxLength(50).IsRequired();

        builder.Property(p => p.Province).HasMaxLength(10).IsRequired().IsUnicode(false);
        builder.Property(p => p.FiscalCode).HasMaxLength(50).IsRequired(false).IsUnicode(false);

        builder.Property(p => p.CellphoneNumber).HasMaxLength(50).IsRequired(false).IsUnicode(false);
        builder.Property(p => p.EmailAddress).HasMaxLength(256).IsRequired(false);

        builder.ToTable("People");
        base.Configure(builder);
    }
}