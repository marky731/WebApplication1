using DataAccess.DbContext;
using EntityLayer.Models;
using Intermediary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class AddressRepository(AppDbContext context) : GenericRepository<Address>(context), IAddressRepository
    {
        public override async Task<IEnumerable<Address>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .OrderBy(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
} 