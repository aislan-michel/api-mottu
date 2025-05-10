using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Mottu.Api.Domain.Entities;

namespace Mottu.Api.Infrastructure.Identity;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<DeliveryMan> DeliveryMen { get; set; }
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Rent> Rents { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(string) && property.GetColumnType() == null)
                {
                    property.SetColumnType("varchar(255)");
                }
            }
        }
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.SetUpdatedAt(DateTime.Now);

            if (entry.State == EntityState.Added)
                entry.Entity.SetCreatedAt(DateTime.Now);
        }

        return base.SaveChanges();
    }
}
