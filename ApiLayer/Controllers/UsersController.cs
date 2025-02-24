using Intermediary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FluentValidation;

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
    public async Task<ApiResponse<PaginatedResponse<List<UserDto>>>> GetUsers([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        return await _userService.GetAllUsers(pageNumber, pageSize);
    }

    [HttpGet("{userId}")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<UserDto>> GetSingleUser(int userId)
    {
        if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value != userId.ToString())
            return new ApiResponse<UserDto>(false, "Unauthorized", null);
        return await _userService.GetUserById(userId);
    }

    [HttpPut()]
    [Authorize]
    public async Task<ApiResponse<UserDto>> EditUser(UserDto userDto)
    {
        if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value != userDto.Id.ToString())
            return new ApiResponse<UserDto>(false, "Unauthorized", null);
        return await _userService.UpdateUser(userDto);
    }

    [HttpPost()]
    public async Task<ApiResponse<UserToAddDto>> AddUser(UserToAddDto userToAddDto)
    {
        return await _userService.CreateUser(userToAddDto);
    }

    [HttpDelete("")]
    [Authorize(Roles = "admin")]
    public async Task<ApiResponse<string?>> DeleteUser(int userId)
    {
        if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value != userId.ToString())
            return new ApiResponse<string?>(false, "Unauthorized", null);
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
}