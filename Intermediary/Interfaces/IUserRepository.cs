using EntityLayer.Dtos;
using EntityLayer.Models;

namespace Intermediary.Interfaces
{
    public interface IUserRepository
    {
        List<user> GetAllUsers();
        user? GetUserById(int id);
        void AddUser(user userToAddDto);
        void UpdateUser(user? user);
        void DeleteUser(int id);
    }
}
