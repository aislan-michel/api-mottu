using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Mottu.Api.Domain.Entities;
using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Infrastructure.Configurations;

public class DeliveryManConfiguration : EntityTypeConfigurationBase<DeliveryMan>
{
    public override void Configure(EntityTypeBuilder<DeliveryMan> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired().HasColumnName("name").HasColumnType("varchar(255)");
        builder.Property(x => x.CompanyRegistrationNumber).IsRequired().HasColumnName("company_registration_number").HasColumnType("varchar(14)");
        builder.Property(x => x.DateOfBirth).IsRequired().HasColumnName("date_of_birth").HasColumnType("date");

        builder.OwnsOne(x => x.DriverLicense, y =>
        {
            y.Property(x => x.Number).IsRequired().HasColumnName("driver_license_number").HasColumnType("varchar(11)");
            y.Property(x => x.Type).IsRequired().HasColumnName("driver_license_type").HasColumnType("char(1)");
            y.Property(x => x.ImagePath).HasColumnName("driver_license_image_path").HasColumnType("varchar(255)");
        });

        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");
        builder
          .HasOne<ApplicationUser>()
          .WithOne()
          .HasForeignKey<DeliveryMan>(x => x.UserId)
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.RentId).HasColumnName("rent_id").HasColumnType("varchar(255)");
        builder
            .HasOne(x => x.Rent)
            .WithOne(x => x.DeliveryMan)
            .HasForeignKey<DeliveryMan>(x => x.RentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("delivery_men");
    }
}