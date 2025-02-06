using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace Intermediary.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
    }
}