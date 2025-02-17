using DataAccess.DbContext;
using EntityLayer.Models;
using Intermediary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        public override async Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Addresses)
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public override async Task AddAsync(User entity)
        {
            if (entity.Addresses != null && entity.Addresses.Any())
            {
                foreach (var address in entity.Addresses)
                {
                    await _context.Addresses.AddAsync(address);
                }
            }
            await base.AddAsync(entity);
        }
    }
}