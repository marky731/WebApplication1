using DataAccess.DbContext;
using EntityLayer.Models;
using Intermediary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ProfilePicRepository(AppDbContext context) : GenericRepository<ProfilePic>(context), IProfilePicRepository
    {
        public override async Task<IEnumerable<ProfilePic>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
} 