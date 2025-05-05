using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Mottu.Api.Domain.Entities;

namespace Mottu.Api.Infrastructure.Configurations;

public class MotorcycleConfiguration : EntityTypeConfigurationBase<Motorcycle>
{
    public override void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Year).IsRequired().HasColumnType("number(4)");
        builder.Property(x => x.Model).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.Plate).IsRequired().HasColumnType("varchar(7)");

        builder.ToTable("motorcycles");
    }
}