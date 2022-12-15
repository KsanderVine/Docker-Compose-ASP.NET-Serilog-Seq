using Microsoft.EntityFrameworkCore;
using UserBlogApp.Models;

namespace UserBlogApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelPost(modelBuilder);
            ModelUser(modelBuilder);

            static void ModelPost(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Post>()
                    .HasKey(e => e.Id);

                modelBuilder.Entity<Post>()
                    .Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()");

                modelBuilder.Entity<Post>()
                    .HasOne(e => e.Author)
                    .WithMany(e => e.Posts)
                    .HasForeignKey(e => e.AuthorId);

                modelBuilder.Entity<Post>()
                    .Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity<Post>()
                    .Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            }

            static void ModelUser(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>()
                    .HasKey(e => e.Id);

                modelBuilder.Entity<User>()
                    .Property(e => e.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()");

                modelBuilder.Entity<User>()
                    .Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                modelBuilder.Entity<User>()
                    .Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            }
        }
    }
}
