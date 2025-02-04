using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class UserService : IUserService
{
    PostgreManager _context;

    public UserService(IConfiguration config)
    {
        _context = new PostgreManager(config);
    }

    public ApiResponse<IEnumerable<User>> GetAllUsers()
    {
        string sql = @"
            SELECT *
            FROM TutorialAppSchema.Users ORDER BY id ASC";
        IEnumerable<User> users = _context.LoadData<User>(sql);
        return new ApiResponse<IEnumerable<User>>(true, "Users retrieved successfully", users);
    }

    public ApiResponse<User> GetUserById(int userId)
    {
        string sql = @"
            SELECT *
            FROM TutorialAppSchema.Users
                WHERE id = " + userId.ToString(); //"7"
        User user = _context.LoadDataSingle<User>(sql);
        return new ApiResponse<User>(true, "User retrieved successfully", user);
    }

    public ApiResponse<string> CreateUser(UserToAddDto userDto)
    {
        var user = new User
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Gender = userDto.Gender,
            Active = userDto.Active
        };
        
        string sql = @"
            INSERT INTO TutorialAppSchema.Users(
                FirstName,
                LastName,
                Email,
                Gender,
                Active
            ) VALUES (" +
                     "'" + user.FirstName +
                     "', '" + user.LastName +
                     "', '" + user.Email +
                     "', '" + user.Gender +
                     "', '" + user.Active +
                     "')";

        Console.WriteLine(sql);

        if (_context.ExecuteSql(sql))
        {
            return new ApiResponse<string>(true, "User added successfully", null);
        }

        throw new Exception("Failed to Add User");

    }

    public ApiResponse<User> UpdateUser(User user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Users
            SET FirstName = '" + user.FirstName +
                     "', LastName = '" + user.LastName +
                     "', Email = '" + user.Email +
                     "', Gender = '" + user.Gender +
                     "', Active = '" + user.Active +
                     "' WHERE id = " + user.Id;

        Console.WriteLine(sql);

        if (_context.ExecuteSql(sql))
        {
            return new ApiResponse<User>(true, "User updated successfully", user);
        }

        throw new Exception("Failed to Update User");
    }

    public ApiResponse<string> DeleteUser(int userId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.Users 
                WHERE id = " + userId.ToString();

        Console.WriteLine(sql);

        if (_context.ExecuteSql(sql))
        {
            return new ApiResponse<string>(true, "User deleted successfully", null);
        }

        throw new Exception("Failed to Delete User");
    }

    IEnumerable<User> IUserService.GetAllUsers()
    {
        throw new NotImplementedException();
    }

    User IUserService.GetUserById(int id)
    {
        throw new NotImplementedException();
    }

    void IUserService.CreateUser(User user)
    {
        throw new NotImplementedException(); //nedenini arastir
    }

    void IUserService.UpdateUser(User user)
    {
        throw new NotImplementedException();
    }

    void IUserService.DeleteUser(int id)
    {
        throw new NotImplementedException();
    }
}