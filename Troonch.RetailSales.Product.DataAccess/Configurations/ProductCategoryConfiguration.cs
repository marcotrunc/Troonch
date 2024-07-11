using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troonch.DataAccess.Base;
using Troonch.Sales.Domain.Entities;

namespace Troonch.Sales.DataAccess.Configurations
{
    public class ProductCategoryConfiguration :  BaseEntityConfiguration<ProductCategory>
    {
        public override void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            base.Configure(builder);

            builder.Property(pc => pc.Name).IsRequired().HasMaxLength(128);

            builder.HasIndex(pc => pc.Name);

            builder.HasOne(pc => pc.ProductSizeType)
                .WithMany(pst => pst.ProductCategories)
                .HasForeignKey(pc => pc.ProductSizeTypeId)
                .IsRequired();

            builder.HasMany(pc => pc.Products)
                .WithOne(p => p.ProductCategory)
                .HasForeignKey(p => p.ProductCategoryId)
                .IsRequired()
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);

            builder.HasMany(pc => pc.ProductGenders)
                .WithOne(pgc => pgc.ProductCategory)
                .HasForeignKey(pgc => pgc.ProductCategoryId)
                .IsRequired();
        }
    }
}
