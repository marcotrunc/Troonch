
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Troonch.DataAccess.Base;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Sales.DataAccess.Configurations;

public class ProductConfiguration : BaseEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(128);
        builder.Property(p => p.Description).IsRequired(false).HasMaxLength(256);
        builder.Property(b => b.Slug).IsRequired().HasMaxLength(50);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);
        builder.Property(p => p.CoverImageLink).IsRequired(false);
        builder.Property(p => p.ProductMaterialId).IsRequired(false).HasDefaultValue(null);

        builder.HasIndex(p => p.Name).IsUnique();
        builder.HasIndex(p => p.Slug).IsUnique();
        builder.HasIndex(p => p.CoverImageLink).IsUnique();

        builder.HasMany(p => p.ProductItems)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.ProductGender)
            .WithMany(pg => pg.Products)
            .HasForeignKey(p => p.ProductGenderId)
            .IsRequired();

        builder.HasOne(p => p.ProductBrand)
            .WithMany(pb => pb.Products)
            .HasForeignKey(pb => pb.ProductBrandId)
            .IsRequired();

        builder.HasOne(p => p.ProductCategory)
            .WithMany(pc => pc.Products)
            .HasForeignKey(pb => pb.ProductCategoryId)
            .IsRequired();

        builder.HasOne(p => p.ProductMaterial)
            .WithMany(pc => pc.Products)
            .HasForeignKey(pb => pb.ProductMaterialId)
            .IsRequired(false);

        builder.HasMany(p => p.ProductTags)
            .WithOne(ptl => ptl.Product)
            .HasForeignKey(ptl => ptl.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
