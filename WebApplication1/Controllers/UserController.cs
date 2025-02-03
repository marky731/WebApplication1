using Microsoft.AspNetCore.Mvc;
using WebApplication1.DatabaseService;
using WebApplication1.Dtos;
using WebApplication1.Models;

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
        public IEnumerable<User> GetUsers()
        {
            string sql = @"
            SELECT *
            FROM TutorialAppSchema.Users";
            IEnumerable<User> users = _dapper.LoadData<User>(sql);
            return users;
        }

        [HttpGet("{userId}")]
        public User GetSingleUser(int userId)
        {
            string sql = @"
            SELECT *
            FROM TutorialAppSchema.Users
                WHERE id = " + userId.ToString(); //"7"
            User user = _dapper.LoadDataSingle<User>(sql);
            return user;
        }

        [HttpPut()]
        public IActionResult EditUser(User user)
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
                return Ok();
            }

            throw new Exception("Failed to Update User");
        }

        [HttpPost()]
        public IActionResult AddUser(UserToAddDto user)
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
                return Ok();
            }

            throw new Exception("Failed to Add User");
        }

        [HttpDelete()]
        public IActionResult DeleteUser(int userId)
        {
            string sql = @"
            DELETE FROM TutorialAppSchema.Users 
                WHERE id = " + userId.ToString();

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Delete User");
        }
    }
}