using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Mottu.Api.Domain.Entities;

namespace Mottu.Api.Infrastructure.Configurations;

public class RentConfiguration : EntityTypeConfigurationBase<Rent>
{
    public override void Configure(EntityTypeBuilder<Rent> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.DeliveryManId).IsRequired().HasColumnName("delivery_man_id").HasColumnType("varchar(255)");
        builder
          .HasOne(x => x.DeliveryMan)
          .WithOne(x => x.Rent)
          .HasForeignKey<Rent>(x => x.DeliveryManId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.MotorcycleId).IsRequired().HasColumnName("motorcycle_id").HasColumnType("varchar(255)");
        builder
          .HasOne(x => x.Motorcycle)
          .WithOne(x => x.Rent)
          .HasForeignKey<Rent>(x => x.MotorcycleId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.StartDate).IsRequired().HasColumnName("start_date").HasColumnType("datetime");
        builder.Property(x => x.EndDate).IsRequired().HasColumnName("end_date").HasColumnType("datetime"); ;
        builder.Property(x => x.ExpectedEndDate).IsRequired().HasColumnName("expected_end_date").HasColumnType("datetime"); ;

        builder.OwnsOne(x => x.Plan, y =>
        {
            y.Property(x => x.Days).IsRequired().HasColumnName("plan_days").HasColumnType("varchar(255)");
            y.Property(x => x.DailyRate).IsRequired().HasColumnName("plan_daily_rate").HasColumnType("decimal(18,2)");
        });

        builder.Property(x => x.ReturnDate).HasColumnName("return_date").HasColumnType("datetime");
        builder.Property(x => x.TotalAmountPayable).HasColumnName("total_amount_payable").HasColumnType("decimal(18,2)");

        builder.ToTable("rents");
    }
}