using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Troonch.DataAccess.Base;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Sales.DataAccess.Configurations;

public class ProductGenderConfiguration : BaseEntityConfiguration<ProductGender>
{
    public override void Configure(EntityTypeBuilder<ProductGender> builder)
    { 
        base.Configure(builder);

        builder.Property(pg => pg.Name).IsRequired().HasMaxLength(128);

        builder.HasIndex(pg => pg.Name).IsUnique();

        builder.HasMany(pg => pg.Products)
            .WithOne(p => p.ProductGender)
            .HasForeignKey(pg => pg.ProductGenderId)
            .IsRequired();

        builder.HasMany(pg => pg.ProductCategories)
            .WithOne(psl =>psl.ProductGender)
            .HasForeignKey(psl => psl.ProductGenderId)
            .IsRequired();
    }
}
