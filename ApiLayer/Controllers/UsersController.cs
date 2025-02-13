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

        public UsersController(IUserService userService) // basina i konulmaz
        {
            _userService = userService;
        }

        [HttpGet()]
        public ApiResponse<List<UserDto>> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) => _userService.GetAllUsers(pageNumber, pageSize);

        [HttpGet("{userId}")]
        public ApiResponse<UserDto> GetSingleUser(int userId) => _userService.GetUserById(userId);

        [HttpPut()]
        public ApiResponse<UserDto> EditUser(UserDto userDto) => _userService.UpdateUser(userDto);


        [HttpPost()]
        public ApiResponse<UserToAddDto> AddUser(UserToAddDto userToAddDto) => _userService.CreateUser(userToAddDto);


        [HttpDelete("")]
        public ApiResponse<string?> DeleteUser(int userId) => _userService.DeleteUser(userId);
    }
}