using EntityLayer.Dtos;
using EntityLayer.Models;
using Intermediary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext.AppDbContext _context;

        public UserRepository(DbContext.AppDbContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers(int pageNumber, int pageSize)
        {
            return _context.Users
                .Include(u => u.Role)
                .Include(u => u.Addresses)
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public User? GetUserById(int id)
        {
            return _context.Users.Include(u => u.Role).Include(u => u.Addresses).FirstOrDefault(u => u.Id == id);
        }


        public void AddUser(User user)
        {
            if (user.Addresses != null && user.Addresses.Any())
            {
                foreach (var address in user.Addresses)
                {
                    _context.Addresses.Add(address);
                }
            }
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
