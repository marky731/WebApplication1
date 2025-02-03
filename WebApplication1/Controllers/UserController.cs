using Microsoft.AspNetCore.Mvc;
using WebApplication1.DatabaseService;
using WebApplication1.Dtos;
using WebApplication1.Models;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserService _dapper;

        public UserController(IConfiguration config)
        {
            _dapper = new UserService(config);
        }

        [HttpGet()]
        public ApiResponse<IEnumerable<User>> GetUsers()
        {
            string sql = @"
            SELECT *
            FROM TutorialAppSchema.Users";
            IEnumerable<User> users = _dapper.LoadData<User>(sql);
            return new ApiResponse<IEnumerable<User>>(true, "Users retrieved successfully", users);
        }

        [HttpGet("{userId}")]
        public ApiResponse<User> GetSingleUser(int userId)
        {
            string sql = @"
            SELECT *
            FROM TutorialAppSchema.Users
                WHERE id = " + userId.ToString(); //"7"
            User user = _dapper.LoadDataSingle<User>(sql);
            return new ApiResponse<User>(true, "User retrieved successfully", user);
        }

        [HttpPut()]
        public ApiResponse<User> EditUser(User user)
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

            if (_dapper.ExecuteSql(sql))
            {
                return new ApiResponse<User>(true, "User updated successfully", user);
            }

            throw new Exception("Failed to Update User");
        }

        [HttpPost()]
        public ApiResponse<string> AddUser(UserToAddDto user)
        {
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

            if (_dapper.ExecuteSql(sql))
            {
                return new ApiResponse<string>(true, "User added successfully", null);
            }

            throw new Exception("Failed to Add User");
        }

        [HttpDelete()]
        public ApiResponse<string> DeleteUser(int userId)
        {
            string sql = @"
            DELETE FROM TutorialAppSchema.Users 
                WHERE id = " + userId.ToString();

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return new ApiResponse<string>(true, "User deleted successfully", null);
            }

            throw new Exception("Failed to Delete User");
        }
    }
}