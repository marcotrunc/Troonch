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
    public class ProductColorConfiguration : BaseEntityConfiguration<ProductColor>
    {
        public override void Configure(EntityTypeBuilder<ProductColor> builder)
        {
            base.Configure(builder);

            builder.Property(pcol=> pcol.Name).IsRequired().HasMaxLength(80);
            builder.Property(pcol => pcol.HexadecimalValue).IsRequired(false).HasPrecision(7);

            builder.HasMany(pcol => pcol.ProductItems)
                .WithOne(pi => pi.ProductColor)
                .HasForeignKey(pi => pi.ProductColorId)
                .IsRequired();
        }
    }
}
