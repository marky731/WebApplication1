using AutoMapper;
using Intermediary.Interfaces;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using EntityLayer.Models;
using Microsoft.AspNetCore.Hosting;


namespace Intermediary.Services
{
    public class EfImageService : IProfilePicService
    {
        private readonly IProfilePicRepository _profilePicRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EfImageService(IProfilePicRepository profilePicRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _profilePicRepository = profilePicRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ApiResponse<List<ImageDto>>> GetAllProfilePics(int pageNumber, int pageSize)
        {
            var profilePics = await _profilePicRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            var totalCount = await _profilePicRepository.GetTotalCountAsync();
            var profilePicDtos = _mapper.Map<List<ImageDto>>(profilePics);
            return new ApiResponse<List<ImageDto>>(true, "Profile pictures retrieved successfully", profilePicDtos);
        }

        public async Task<ApiResponse<ImageDto>> GetProfilePicById(int profilePicId)
        {
            var profilePic = await _profilePicRepository.GetByIdAsync(profilePicId);
            if (profilePic == null)
                throw new Exception("Profile picture not found");

            var profilePicDto = _mapper.Map<ImageDto>(profilePic);
            return new ApiResponse<ImageDto>(true, "Profile picture retrieved successfully", profilePicDto);
        }

        public async Task<ApiResponse<ImageUploadDto?>> CreateProfilePic(ImageUploadDto uploadDto)
        {

            // Generate unique file name
            var fileName = Guid.NewGuid() + Path.GetExtension(uploadDto.File.FileName);

            // Ensure directory exists
            Console.WriteLine($"WebRootPath: {_webHostEnvironment.WebRootPath}");

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "profile_pictures");
            Directory.CreateDirectory(uploadsFolder);

            // Save file to server
            await using (var stream = new FileStream(Path.Combine(uploadsFolder, fileName), FileMode.Create))
            {
                await uploadDto.File.CopyToAsync(stream);
            }


            // Save to database
            var profilePicDto = new ImageDto
            {
                UserId = uploadDto.UserId,
                ImagePath = "/profile_pictures/" + fileName
            };


            var profilePic = _mapper.Map<Image>(profilePicDto);
            await _profilePicRepository.AddAsync(profilePic);
            await _profilePicRepository.SaveChangesAsync();
            return new ApiResponse<ImageUploadDto?>(true, "Profile picture added successfully", uploadDto);
        }

        public async Task<ApiResponse<ImageDto>> UpdateProfilePic(ImageDto imageDto)
        {
            var profilePic = await _profilePicRepository.GetByIdAsync(imageDto.Id);
            if (profilePic == null)
                throw new Exception("Profile picture not found");

            _mapper.Map(imageDto, profilePic);
            await _profilePicRepository.UpdateAsync(profilePic);
            await _profilePicRepository.SaveChangesAsync();
            return new ApiResponse<ImageDto>(true, "Profile picture updated successfully", imageDto);
        }

        public async Task<ApiResponse<string?>> DeleteProfilePic(int profilePicId)
        {
            await _profilePicRepository.DeleteAsync(profilePicId);
            await _profilePicRepository.SaveChangesAsync();
            return new ApiResponse<string?>(true, "Profile picture deleted successfully", null);
        }
    }
}