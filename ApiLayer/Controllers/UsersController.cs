using Intermediary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet()]
        public async Task<ApiResponse<List<UserDto>>> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await _userService.GetAllUsers(pageNumber, pageSize);
        }

        [HttpGet("{userId}")]
        public async Task<ApiResponse<UserDto>> GetSingleUser(int userId)
        {
            return await _userService.GetUserById(userId);
        }

        [HttpPut()]
        public async Task<ApiResponse<UserDto>> EditUser(UserDto userDto)
        {
            return await _userService.UpdateUser(userDto);
        }

        [HttpPost()]
        public async Task<ApiResponse<UserToAddDto>> AddUser(UserToAddDto userToAddDto)
        {
            return await _userService.CreateUser(userToAddDto);
        }

        [HttpDelete("")]
        public async Task<ApiResponse<string?>> DeleteUser(int userId)
        {
            return await _userService.DeleteUser(userId);
        }
    }
}