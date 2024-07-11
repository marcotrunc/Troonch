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
    public class ProductSizeTypeConfiguration : BaseEntityConfiguration<ProductSizeType>
    {
        public override void Configure(EntityTypeBuilder<ProductSizeType> builder)
        {
            base.Configure(builder);

            builder.Property(pst => pst.Name).IsRequired().HasMaxLength(128);

            builder.HasMany(pst => pst.ProductSizeOptions)
                .WithOne(pso => pso.ProductSizeType)
                .HasForeignKey(pso => pso.ProductSizeTypeId)
                .IsRequired();

            builder.HasMany(pst => pst.ProductCategories)
                .WithOne(pc => pc.ProductSizeType)
                .HasForeignKey(pc => pc.ProductSizeTypeId)
                .IsRequired();

        }
    }

}
