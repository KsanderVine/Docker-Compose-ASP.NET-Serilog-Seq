using Microsoft.EntityFrameworkCore;
using UserBlogApp.Models;

namespace UserBlogApp.Data
{
    public static class AppDbContextSeed
    {
        public static async Task Seed(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();

            await SeedDataAsync(scope, env);
        }

        private static async Task SeedDataAsync(IServiceScope scope, IWebHostEnvironment env)
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("AppDbContextSeed");

            if (env.IsProduction())
            {
                logger.LogInformation("Attempting to apply migrations...");
                try
                {
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Could not apply migrations");
                }
            }

            if (env.IsDevelopment())
            {
                var utcNow = DateTime.UtcNow;

                await context.Users.AddRangeAsync(
                    new User
                    {
                        Username = "Jon Smith",
                        CreatedAt = utcNow,
                        UpdatedAt = utcNow,

                        Posts = new List<Post> {
                            new Post
                            {
                                Content = "This is simplest post ever",
                                CreatedAt = utcNow,
                                UpdatedAt = utcNow
                            }}
                    });
                await context.SaveChangesAsync();
            }
        }
    }
}
