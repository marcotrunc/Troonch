using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Troonch.DataAccess.Base;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Sales.DataAccess.Configurations;

public class ProductBrandConfiguration : BaseEntityConfiguration<ProductBrand>
{
    public override void Configure(EntityTypeBuilder<ProductBrand> builder)
    {
        base.Configure(builder);

        builder.Property(pb => pb.Name).IsRequired().HasMaxLength(128);
        builder.Property(pb => pb.Description).IsRequired(false).HasMaxLength(256);
        builder.Property(b => b.Slug).IsUnicode().IsRequired().HasMaxLength(128);

        builder.HasIndex(b => b.Name).IsUnique();
        builder.HasIndex(b => b.Slug).IsUnique();

        builder.HasMany(pb => pb.Products)
            .WithOne(p => p.ProductBrand)
            .HasForeignKey(p => p.ProductBrandId)
            .IsRequired();
    }
}
