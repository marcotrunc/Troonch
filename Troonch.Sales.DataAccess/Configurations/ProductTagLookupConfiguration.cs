using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Sales.DataAccess.Configurations
{
    public class ProductTagLookupConfiguration : IEntityTypeConfiguration<ProductTagLookup>
    {
        public  void Configure(EntityTypeBuilder<ProductTagLookup> builder)
        {
            builder.HasKey(ptl => new { ptl.ProductId,ptl.TagId });

            builder.HasOne(ptl => ptl.Product)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(ptl => ptl.ProductId)
                .IsRequired();

            builder.HasOne(ptl => ptl.ProductTag)
                .WithMany(pt => pt.ProductTags)
                .HasForeignKey(ptl => ptl.TagId)
                .IsRequired();
        }
    }
}
