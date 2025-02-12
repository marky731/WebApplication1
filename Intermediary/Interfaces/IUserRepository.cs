using EntityLayer.Dtos;
using EntityLayer.Models;

namespace Intermediary.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User? GetUserById(int id);
        void AddUser(User userToAddDto);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
