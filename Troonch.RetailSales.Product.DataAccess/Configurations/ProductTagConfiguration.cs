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
    public class ProductTagConfiguration : BaseEntityConfiguration<ProductTag>
    {
        public override void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            base.Configure(builder);

            builder.Property(pt => pt.Value).IsRequired().HasMaxLength(100);

            builder.HasMany(pt => pt.ProductTags)
                .WithOne(ptl => ptl.ProductTag)
                .HasForeignKey(ptl => ptl.TagId)
                .IsRequired();
        }
    }
}
