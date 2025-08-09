using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrebDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd = false)
        {
            Console.WriteLine("---> Applying migrations...");

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeedData(context, isProd);
            }
        }
        private static void SeedData(AppDbContext context, bool isProd = false)
        {
            if(isProd)
            {
                Console.WriteLine("---> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"---> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("---> Seeding Data...");

                context.Platforms.AddRange(
                    new Platform {Name = "DotNet", Publisher = "Microsoft", Cost = "Free" },
                    new Platform {Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                    new Platform {Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
                context.SaveChanges();
            }

            else
            {
                Console.WriteLine("---> We already have data");
            }
        }

    }
}