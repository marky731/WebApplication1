using Intermediary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using ApiLayer.Authorization;
using System.Security.Claims;

namespace ApiLayer.Controllers;

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
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<PaginatedResponse<List<UserDto>>>> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        return await _userService.GetAllUsers(pageNumber, pageSize);
    }

    [HttpGet("{userId}")]
    [AuthorizeUserId]
    public async Task<ApiResponse<UserDto>> GetSingleUser(int userId)
    {
        return await _userService.GetUserById(userId);
    }

    [HttpPut()]
    [AuthorizeUserId]
    public async Task<ApiResponse<UserDto>> EditUser(UserDto userDto)
    {
        return await _userService.UpdateUser(userDto);
    }

    [HttpPost()]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<UserToAddDto>> AddUser(UserToAddDto userToAddDto)
    {
        return await _userService.CreateUser(userToAddDto);
    }

    [HttpDelete()]
    [AuthorizeUserId]
    public async Task<ApiResponse<string?>> DeleteUser(int userId)
    {
        return await _userService.DeleteUser(userId);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<string>>> Register(UserRegisterDto userRegisterDto)
    {
        return await _userService.RegisterUser(userRegisterDto);
    }

    [HttpPost("login")]
    public async Task<ApiResponse<string>> Login(UserLoginDto userLoginDto)
    {
        return await _userService.LoginUser(userLoginDto);
    }

    [HttpGet("me")]
    [Authorize] 
    public async Task<ApiResponse<UserInfoDto>> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("Invalid token");
        }

        int userId = int.Parse(userIdClaim);
        return await _userService.GetUserInfoById(userId);
    }
}