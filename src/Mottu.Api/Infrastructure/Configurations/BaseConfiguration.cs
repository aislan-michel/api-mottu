using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Mottu.Api.Domain.Entities;

namespace Mottu.Api.Infrastructure.Configurations;

public abstract class EntityTypeConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(x => x.Id).IsRequired().HasColumnName("id").HasColumnType("varchar(255)");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedAt)
            .IsRequired().HasColumnName("created_at").HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("update_at").HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnUpdate();

        builder.Property(e => e.Active)
            .IsRequired().HasColumnName("active").HasColumnType("TINYINT(1)")
            .HasDefaultValue(false);
    }
}
