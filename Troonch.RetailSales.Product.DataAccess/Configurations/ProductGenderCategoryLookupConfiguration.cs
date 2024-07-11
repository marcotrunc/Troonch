using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Sales.DataAccess.Configurations
{
    public class ProductGenderCategoryLookupConfiguration : IEntityTypeConfiguration<ProductGenderCategoryLookup>
    {
        public  void Configure(EntityTypeBuilder<ProductGenderCategoryLookup> builder)
        {
            builder.HasKey(pgcl => new { pgcl.ProductGenderId, pgcl.ProductCategoryId});

            builder.HasOne(pgcl => pgcl.ProductGender)
                .WithMany(pg => pg.ProductCategories)
                .HasForeignKey(pgcl => pgcl.ProductGenderId)
                .IsRequired();

            builder.HasOne(pgcl => pgcl.ProductCategory)
                .WithMany(pst => pst.ProductGenders)
                .HasForeignKey(pgcl => pgcl.ProductCategoryId)
                .IsRequired();
        }
    }
}
