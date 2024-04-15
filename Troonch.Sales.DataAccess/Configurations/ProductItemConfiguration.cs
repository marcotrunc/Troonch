using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Troonch.DataAccess.Base;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Sales.DataAccess.Configurations
{
    public class ProductItemConfiguration : BaseEntityConfiguration<ProductItem>
    {
        public override void Configure(EntityTypeBuilder<ProductItem> builder)
        {
            base.Configure(builder);

            builder.Property(pi => pi.OriginalPrice).HasPrecision(18, 2).HasDefaultValue(Decimal.Zero);
            builder.Property(pi => pi.SalePrice).HasPrecision(18, 2).HasDefaultValue(Decimal.Zero);
            builder.Property(pi => pi.Barcode).IsRequired(false).HasMaxLength(30).IsUnicode();
            builder.Property(pi => pi.QuantityAvailable).IsRequired().HasDefaultValue(0);

            builder.HasOne(pi => pi.Product)
                .WithMany(p => p.ProductItems)
                .HasForeignKey(p => p.ProductId)
                .IsRequired();

            builder.HasOne(pi => pi.ProductSizeOption)
                .WithMany(pso => pso.ProductItems)
                .HasForeignKey(pi => pi.ProductSizeOptionId)
                .IsRequired();

            builder.HasOne(pi => pi.ProductColor)
                .WithMany(pcol => pcol.ProductItems)
                .HasForeignKey(pi => pi.ProductColorId)
                .IsRequired();
        }
    }
}
