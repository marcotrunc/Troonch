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
    public class ProductSizeOptionConfiguration : BaseEntityConfiguration<ProductSizeOption>
    {
        public override void Configure(EntityTypeBuilder<ProductSizeOption> builder)
        {
            base.Configure(builder);

            builder.Property(pso => pso.Value).IsRequired().HasMaxLength(50);
            builder.Property(pso => pso.Sort);


            builder.HasOne(pso => pso.ProductSizeType)
                .WithMany(pst => pst.ProductSizeOptions)
                .HasForeignKey(pso => pso.ProductSizeTypeId)
                .IsRequired();

            builder.HasMany(pso => pso.ProductItems)
                .WithOne(pi => pi.ProductSizeOption)
                .HasForeignKey(pi => pi.ProductSizeOptionId)
                .IsRequired();
        }
    }
}
