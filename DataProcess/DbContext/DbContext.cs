using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContext
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<user> users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<user>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
        }
    }
}