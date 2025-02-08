using Intermediary.Interfaces;
using Presentation.Models;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext.DbContext _context;

        public UserRepository(DbContext.DbContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            return _context.users.OrderBy(u => u.Id).ToList();
        }

        public User? GetUserById(int id)
        {
            return _context.users.Find(id);
        }

        public void AddUser(User? user)
        {
            _context.users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User? user)
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
