using Intermediary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilePicsController : ControllerBase
    {
        private readonly IProfilePicService _profilePicService;

        public ProfilePicsController(IProfilePicService profilePicService)
        {
            _profilePicService = profilePicService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<ProfilePicDto>>> GetProfilePics([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await _profilePicService.GetAllProfilePics(pageNumber, pageSize);
        }

        [HttpGet("{profilePicId}")]
        public async Task<ApiResponse<ProfilePicDto>> GetSingleProfilePic(int profilePicId)
        {
            return await _profilePicService.GetProfilePicById(profilePicId);
        }

        [HttpPut]
        public async Task<ApiResponse<ProfilePicDto>> EditProfilePic(ProfilePicDto profilePicDto)
        {
            return await _profilePicService.UpdateProfilePic(profilePicDto);
        }

        [HttpPost]
        public async Task<ApiResponse<ProfilePicDto>> AddProfilePic(ProfilePicDto profilePicDto)
        {
            return await _profilePicService.CreateProfilePic(profilePicDto);
        }

        [HttpDelete]
        public async Task<ApiResponse<string?>> DeleteProfilePic(int profilePicId)
        {
            return await _profilePicService.DeleteProfilePic(profilePicId);
        }
    }
} 