using EntityLayer.Dtos;
using EntityLayer.Models;
using Intermediary.Interfaces;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext.AppDbContext _context;

        public UserRepository(DbContext.AppDbContext context)
        {
            _context = context;
        }

        public List<user> GetAllUsers()
        {
            return _context.users.OrderBy(u => u.Id).ToList();
        }

        public user? GetUserById(int id)
        {
            return _context.users.Find(id);
        }

        
        public void AddUser(user user)
        {
            _context.users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(user? user)
        {
            _context.users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.users.Find(id);
            if (user != null)
            {
                _context.users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
