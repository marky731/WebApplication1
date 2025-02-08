using Intermediary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.ApiResponse;
using Presentation.Dtos;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _iUserService; 

        public UsersController(IUserService iUserService)
        {
            _iUserService = iUserService;
        }

        [HttpGet()]
        public ApiResponse<List<UserDto>> GetUsers() => _iUserService.GetAllUsers();

        [HttpGet("{userId}")]
        public ApiResponse<UserDto> GetSingleUser(int userId) => _iUserService.GetUserById(userId);

        [HttpPut()]
        public ApiResponse<UserDto> EditUser(UserDto userDto) => _iUserService.UpdateUser(userDto);


        [HttpPost()]
        public ApiResponse<UserDto> AddUser(UserDto userDto) => _iUserService.CreateUser(userDto);


        [HttpDelete("")]
        public ApiResponse<string?> DeleteUser(int userId) => _iUserService.DeleteUser(userId);
    }
}