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
    [Authorize] // Secure endpoint - Only Admin can access
    public async Task<ApiResponse<List<UserDto>>> GetUsers([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        return await _userService.GetAllUsers(pageNumber, pageSize);
    }

    [HttpGet("{userId}")]
    [Authorize] // Secure endpoint
    public async Task<ApiResponse<UserDto>> GetSingleUser(int userId)
    {
        // Get the user ID from the token
        var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Check if the user is authorized to access this resource
        if (userIdFromToken != userId.ToString() && !User.IsInRole("admin"))
        {
            return new ApiResponse<UserDto>(false, "Unauthorized", null);
        }

        return await _userService.GetUserById(userId);
    }
    
    [HttpGet("me")]
    [Authorize] // Secure endpoint - Requires authentication
    public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetCurrentUser()
    {
        // Get the user ID from the token
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _userService.GetUserInfoById(userId);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPut()]
    [Authorize] // Secure endpoint
    public async Task<ApiResponse<UserDto>> EditUser(UserDto userDto)
    {
        // Get the user ID from the token
        var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Check if the user is authorized to access this resource
        if (userIdFromToken != userDto.Id.ToString() && !User.IsInRole("admin"))
        {
            return new ApiResponse<UserDto>(false, "Unauthorized", null);
        }

        return await _userService.UpdateUser(userDto);
    }

    [HttpPost()]
    public async Task<ApiResponse<UserToAddDto>> AddUser(UserToAddDto userToAddDto)
    {
        return await _userService.CreateUser(userToAddDto);
    }

    [HttpDelete("")]
    [Authorize(Roles = "admin")] // Secure endpoint - Only Admin can access
    public async Task<ApiResponse<string?>> DeleteUser(int userId)
    {
        return await _userService.DeleteUser(userId);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<string>>> Register(UserRegisterDto userRegisterDto)
    {
        try
        {
            var result = await _userService.RegisterUser(userRegisterDto);
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse<string>(false, "Validation failed", null, validationErrors: ex.Errors.Select(e => e.ErrorMessage).ToList()));        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message, null));
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<string>>> Login(UserLoginDto userLoginDto)
    {
        try
        {
            var result = await _userService.LoginUser(userLoginDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<string>(false, ex.Message, null));
        }
    }
}