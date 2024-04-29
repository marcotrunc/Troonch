using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Troonch.DataAccess.Base;
using Troonch.Person.Domain.Entities;

namespace Troonch.Person.DataAccess.Configurations;
public class AddressConfiguration : BaseEntityConfiguration<Address>
{
    public override void Configure(EntityTypeBuilder<Address> builder)
    {
        base.Configure(builder);

        builder.Property(a => a.AddressNumber).IsRequired().ValueGeneratedOnAdd();
        builder.Property(a => a.City).IsRequired().HasMaxLength(128);
        builder.Property(a => a.Country).IsRequired().HasMaxLength(128);
        builder.Property(a => a.Latitude).IsRequired(false).HasPrecision(18, 6);
        builder.Property(a => a.Longitude).IsRequired(false).HasPrecision(18, 6);
        builder.Property(a => a.PostalCode).IsRequired().HasMaxLength(15);
        builder.Property(a => a.Line).IsRequired().HasMaxLength(50);
        builder.Property(a => a.Region).IsRequired(false).HasMaxLength(128);
        builder.Property(a => a.PhoneNumber).IsRequired(false).HasMaxLength(50);
        builder.Property(a => a.Province).IsRequired(false).HasMaxLength(50);
        builder.Property(a => a.IsDefault).IsRequired().HasDefaultValue(true);
        builder.Property(a => a.Note).IsRequired(false).HasMaxLength(256);
        builder.Property(a => a.IsDeleted).IsRequired().HasDefaultValue(true);

        builder.HasOne(a => a.Customer)
               .WithMany(c => c.Addresses)
               .HasForeignKey(a => a.CustomerId)
               .IsRequired(false);

        builder.HasOne(a => a.AddressType)
               .WithMany(at => at.Addresses)
               .HasForeignKey(a => a.AddressTypeId)
               .IsRequired();
    }
}
