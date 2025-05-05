using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Mottu.Api.Domain.Entities;

namespace Mottu.Api.Infrastructure.Identity;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
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
