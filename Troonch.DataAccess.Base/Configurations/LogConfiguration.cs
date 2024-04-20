using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Troonch.Domain.Base.Entities;

namespace Troonch.DataAccess.Base.Configurations;

public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public  void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.HasKey(l => l.Id);
    }
}


