using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Mottu.Api.Domain.Entities;

namespace Mottu.Api.Infrastructure.Configurations;

public class MotorcycleConfiguration : EntityTypeConfigurationBase<Motorcycle>
{
    public override void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Year).IsRequired().HasColumnName("year").HasColumnType("varchar(4)");
        builder.Property(x => x.Model).IsRequired().HasColumnName("model").HasColumnType("varchar(255)");
        builder.Property(x => x.Plate).IsRequired().HasColumnName("plate").HasColumnType("varchar(7)");

        builder.Property(x => x.RentId).HasColumnName("rent_id").HasColumnType("varchar(255)");
        builder
            .HasOne(x => x.Rent)
            .WithOne(x => x.Motorcycle)
            .HasForeignKey<Motorcycle>(x => x.RentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("motorcycles");
    }
}