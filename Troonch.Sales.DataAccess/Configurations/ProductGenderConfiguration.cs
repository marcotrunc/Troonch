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
    public class ProductGenderConfiguration : BaseEntityConfiguration<ProductGender>
    {
        public override void Configure(EntityTypeBuilder<ProductGender> builder)
        { 
            base.Configure(builder);

            builder.Property(pg => pg.Name).IsRequired().HasMaxLength(128);

            builder.HasMany(pg => pg.Products)
                .WithOne(p => p.ProductGender)
                .HasForeignKey(pg => pg.ProductGenderId)
                .IsRequired();
        }
    }
}
