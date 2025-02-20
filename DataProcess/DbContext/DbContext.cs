using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContext
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Addresses)
                .WithOne()
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Image)
                .WithOne(i => i.User)
                .HasForeignKey<Image>(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<Address>()
            //     .HasKey(a => a.Id);
        }
    }
}