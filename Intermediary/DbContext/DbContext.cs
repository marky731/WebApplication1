using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace Intermediary.DbContext
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
    }
}