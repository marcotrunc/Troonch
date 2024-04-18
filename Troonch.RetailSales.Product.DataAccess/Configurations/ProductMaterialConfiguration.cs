using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Troonch.DataAccess.Base;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Sales.DataAccess.Configurations
{
    public class ProductMaterialConfiguration : BaseEntityConfiguration<ProductMaterial>
    {
        public override void Configure(EntityTypeBuilder<ProductMaterial> builder)
        {
            base.Configure(builder);

            builder.Property(pm => pm.Value).IsRequired().HasMaxLength(128);

            builder.HasMany(pm => pm.Products)
                .WithOne(p => p.ProductMaterial)
                .HasForeignKey(p => p.ProductMaterialId)
                .IsRequired(false);
        }
    }
}
