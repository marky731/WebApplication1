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
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet()]
        public ApiResponse<IEnumerable<User>> GetUsers() => _userService.GetAllUsers();

        [HttpGet("{userId}")]
        public ApiResponse<User> GetSingleUser(int userId) => _userService.GetUserById(userId);

        [HttpPut()]
        public ApiResponse<User> EditUser(User user) => _userService.UpdateUser(user);


        [HttpPost()]
        public ApiResponse<string> AddUser(UserToAddDto userDto) => _userService.CreateUser(userDto);


        [HttpDelete("")]
        public ApiResponse<string> DeleteUser(int userId) => _userService.DeleteUser(userId);
    }
}