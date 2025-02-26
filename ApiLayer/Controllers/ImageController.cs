using ApiLayer.Authorization;
using Intermediary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using EntityLayer.Models;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("{profilePicId}")]
        [AuthorizeUserId]
        public async Task<ApiResponse<ImageDto>> GetSingleProfilePic(int profilePicId)
        {
            return await _profilePicService.GetProfilePicById(profilePicId);
        }

        [HttpPut]
        [AuthorizeUserId]
        public async Task<ApiResponse<ImageDto>> EditProfilePic(ImageDto imageDto)
        {
            return await _profilePicService.UpdateProfilePic(imageDto);
        }

        [HttpPost]
        [AuthorizeUserId]
        public async Task<ApiResponse<ImageUploadDto?>> UploadProfilePic([FromForm] ImageUploadDto uploadDto)
        {
            return await _profilePicService.CreateProfilePic(uploadDto);
        }

        [HttpDelete]
        [AuthorizeUserId]
        public async Task<ApiResponse<string?>> DeleteProfilePic(int profilePicId)
        {
            return await _profilePicService.DeleteProfilePic(profilePicId);
        }
    }
}