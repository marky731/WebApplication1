using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using Intermediary.Interfaces;

namespace AuthorizationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("current-user")]
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
}