using DataAccess.DbContext;
using EntityLayer.Models;
using Intermediary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class RoleRepository(AppDbContext context) : GenericRepository<Role>(context), IRoleRepository
    {
        public override async Task<IEnumerable<Role>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .OrderBy(r => r.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
} 