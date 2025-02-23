using EntityLayer.Models;

namespace Intermediary.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email); 
    }
}
