using AutoMapper;
using Intermediary.Interfaces;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using EntityLayer.Models;

namespace Intermediary.Services
{
    public class EfProfilePicService : IProfilePicService
    {
        private readonly IProfilePicRepository _profilePicRepository;
        private readonly IMapper _mapper;

        public EfProfilePicService(IProfilePicRepository profilePicRepository, IMapper mapper)
        {
            _profilePicRepository = profilePicRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<ImageDto>>> GetAllProfilePics(int pageNumber, int pageSize)
        {
            var profilePics = await _profilePicRepository.GetAllAsync(pageNumber, pageSize);
            var totalCount = await _profilePicRepository.GetTotalCountAsync();
            var profilePicDtos = _mapper.Map<List<ImageDto>>(profilePics);
            return new ApiResponse<List<ImageDto>>(true, "Profile pictures retrieved successfully", profilePicDtos, pageNumber, pageSize, totalCount);
        }

        public async Task<ApiResponse<ImageDto>> GetProfilePicById(int profilePicId)
        {
            var profilePic = await _profilePicRepository.GetByIdAsync(profilePicId);
            if (profilePic == null)
                throw new Exception("Profile picture not found");

            var profilePicDto = _mapper.Map<ImageDto>(profilePic);
            return new ApiResponse<ImageDto>(true, "Profile picture retrieved successfully", profilePicDto);
        }

        public async Task<ApiResponse<ImageDto>> CreateProfilePic(ImageDto imageDto)
        {
            var profilePic = _mapper.Map<Image>(imageDto);
            await _profilePicRepository.AddAsync(profilePic);
            await _profilePicRepository.SaveChangesAsync();
            return new ApiResponse<ImageDto>(true, "Profile picture added successfully", imageDto);
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