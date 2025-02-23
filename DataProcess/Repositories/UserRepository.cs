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

        public override async Task UpdateAsync(User entity)
        {
            var existingUser = await _dbSet
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == entity.Id);

            if (existingUser != null)
            {
                // Detach the existing role to prevent tracking conflicts
                if (existingUser.Role != null)
                {
                    _context.Entry(existingUser.Role).State = EntityState.Detached;
                }

                // Update basic properties
                existingUser.Firstname = entity.Firstname;
                existingUser.Surname = entity.Surname;
                existingUser.Gender = entity.Gender;
                existingUser.RoleId = entity.RoleId;

                // Handle addresses if needed
                if (entity.Addresses != null)
                {
                    // Remove existing addresses
                    _context.Addresses.RemoveRange(existingUser.Addresses);

                    // Add new addresses
                    existingUser.Addresses = entity.Addresses;
                }

                // Update the entity state
                _context.Entry(existingUser).State = EntityState.Modified;
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}