using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Sales.DataAccess.Configurations
{
    public class ProductGenderSizeTypeLookupConfiguration : IEntityTypeConfiguration<ProductGenderSizeTypeLookup>
    {
        public  void Configure(EntityTypeBuilder<ProductGenderSizeTypeLookup> builder)
        {
            builder.HasKey(pgstl => new { pgstl.ProductGenderId, pgstl.ProductSizeTypeId});

            builder.HasOne(pgstl => pgstl.ProductGender)
                .WithMany(pg => pg.ProductSizeTypes)
                .HasForeignKey(pgstl => pgstl.ProductGenderId)
                .IsRequired();

            builder.HasOne(pgstl => pgstl.ProductSizeType)
                .WithMany(pst => pst.ProductGenders)
                .HasForeignKey(pgstl => pgstl.ProductSizeTypeId)
                .IsRequired();
        }
    }
}
