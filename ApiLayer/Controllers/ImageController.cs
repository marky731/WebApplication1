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
        private IWebHostEnvironment _webHostEnvironment;

        public ImageController(IProfilePicService profilePicService, IWebHostEnvironment webHostEnvironment)
        {
            _profilePicService = profilePicService;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<ApiResponse<ImageDto?>> UploadProfilePic([FromForm] ProfilePicUploadDto uploadDto)
        {
            if (uploadDto == null)
            {
                Console.WriteLine("uploadDto is null!");
                return new ApiResponse<ImageDto?>(false, "Invalid request.", null);
            }

            if (uploadDto.File == null || uploadDto.File.Length == 0)
            {
                Console.WriteLine("No file uploaded.");
                return new ApiResponse<ImageDto?>(false, "No file uploaded.", null);
            }

            if (_webHostEnvironment == null)
            {
                Console.WriteLine("_webHostEnvironment is null!");
                return new ApiResponse<ImageDto?>(false, "Server configuration error.", null);
            }

            if (_profilePicService == null)
            {
                Console.WriteLine("_profilePicService is null!");
                return new ApiResponse<ImageDto?>(false, "Service unavailable.", null);
            }

            // Generate unique file name
            string fileName = Guid.NewGuid() + Path.GetExtension(uploadDto.File.FileName);

            // Ensure directory exists
            Console.WriteLine($"WebRootPath: {_webHostEnvironment.WebRootPath}");

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath ?? Directory.GetCurrentDirectory(), "wwwroot", "profile_pictures");
            Directory.CreateDirectory(uploadsFolder);

            // Save file to server
            using (var stream = new FileStream(Path.Combine(uploadsFolder, fileName), FileMode.Create))
            {
                await uploadDto.File.CopyToAsync(stream);
            }


            // Save to database
            var profilePicDto = new ImageDto
            {
                UserId = uploadDto.UserId,
                ImagePath = "/profile_pictures/" + fileName
            };

            var result = await _profilePicService.CreateProfilePic(profilePicDto);

            return new ApiResponse<ImageDto?>(
                true,
                "Profile picture uploaded successfully!",
                result.Data
            );
        }


        [HttpDelete]
        public async Task<ApiResponse<string?>> DeleteProfilePic(int profilePicId)
        {
            return await _profilePicService.DeleteProfilePic(profilePicId);
        }
    }
} 