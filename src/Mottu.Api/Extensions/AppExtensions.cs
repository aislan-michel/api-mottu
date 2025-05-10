using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Mottu.Api.Application.Interfaces;
using Mottu.Api.Application.Models;
using Mottu.Api.Infrastructure.Identity;

namespace Mottu.Api.Extensions;

public static class AppExtensions
{
    public static void Seed(this WebApplication app, IConfiguration configuration)
    {
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;

            var motorcycleUseCase = serviceProvider.GetRequiredService<IMotorcycleUseCase>();
            /*
            using (FileStream openStream = File.OpenRead(@"../../../mocks/motorcycles.json"))
            {
                var motorcycles = JsonSerializer.Deserialize<List<Motorcycle>>(openStream);

                foreach (var motorcycle in motorcycles)
                {
                    
                }
            }
            */
            motorcycleUseCase.Create(new PostMotorcycleRequest()
            {
                Year = 2025,
                Model = "Honda",
                Plate = "NAV9659"
            });
        }
    }

    public static async Task ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var pendingMigrations = await db.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                db.Database.Migrate();
            }
        }
    }

    public static async Task SeedRoles(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "admin", "entregador" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}