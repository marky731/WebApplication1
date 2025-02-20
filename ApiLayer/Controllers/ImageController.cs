using Intermediary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using EntityLayer.Models;

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IProfilePicService _profilePicService;

        public ImageController(IProfilePicService profilePicService)
        {
            _profilePicService = profilePicService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<ImageDto>>> GetProfilePics([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await _profilePicService.GetAllProfilePics(pageNumber, pageSize);
        }

        [HttpGet("{profilePicId}")]
        public async Task<ApiResponse<ImageDto>> GetSingleProfilePic(int profilePicId)
        {
            return await _profilePicService.GetProfilePicById(profilePicId);
        }

        [HttpPut]
        public async Task<ApiResponse<ImageDto>> EditProfilePic(ImageDto imageDto)
        {
            return await _profilePicService.UpdateProfilePic(imageDto);
        }

        [HttpPost("upload")]
        public async Task<ApiResponse<ImageUploadDto?>> UploadProfilePic([FromForm] ImageUploadDto uploadDto)
        {
            return await _profilePicService.CreateProfilePic(uploadDto);

        }

        [HttpDelete]
        public async Task<ApiResponse<string?>> DeleteProfilePic(int profilePicId)
        {
            return await _profilePicService.DeleteProfilePic(profilePicId);
        }
    }
} 