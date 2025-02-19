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

        public async Task<ApiResponse<List<ProfilePicDto>>> GetAllProfilePics(int pageNumber, int pageSize)
        {
            var profilePics = await _profilePicRepository.GetAllAsync(pageNumber, pageSize);
            var totalCount = await _profilePicRepository.GetTotalCountAsync();
            var profilePicDtos = _mapper.Map<List<ProfilePicDto>>(profilePics);
            return new ApiResponse<List<ProfilePicDto>>(true, "Profile pictures retrieved successfully", profilePicDtos, pageNumber, pageSize, totalCount);
        }

        public async Task<ApiResponse<ProfilePicDto>> GetProfilePicById(int profilePicId)
        {
            var profilePic = await _profilePicRepository.GetByIdAsync(profilePicId);
            if (profilePic == null)
                throw new Exception("Profile picture not found");

            var profilePicDto = _mapper.Map<ProfilePicDto>(profilePic);
            return new ApiResponse<ProfilePicDto>(true, "Profile picture retrieved successfully", profilePicDto);
        }

        public async Task<ApiResponse<ProfilePicDto>> CreateProfilePic(ProfilePicDto profilePicDto)
        {
            var profilePic = _mapper.Map<ProfilePic>(profilePicDto);
            await _profilePicRepository.AddAsync(profilePic);
            await _profilePicRepository.SaveChangesAsync();
            return new ApiResponse<ProfilePicDto>(true, "Profile picture added successfully", profilePicDto);
        }

        public async Task<ApiResponse<ProfilePicDto>> UpdateProfilePic(ProfilePicDto profilePicDto)
        {
            var profilePic = await _profilePicRepository.GetByIdAsync(profilePicDto.Id);
            if (profilePic == null)
                throw new Exception("Profile picture not found");

            _mapper.Map(profilePicDto, profilePic);
            await _profilePicRepository.UpdateAsync(profilePic);
            await _profilePicRepository.SaveChangesAsync();
            return new ApiResponse<ProfilePicDto>(true, "Profile picture updated successfully", profilePicDto);
        }

        public async Task<ApiResponse<string?>> DeleteProfilePic(int profilePicId)
        {
            await _profilePicRepository.DeleteAsync(profilePicId);
            await _profilePicRepository.SaveChangesAsync();
            return new ApiResponse<string?>(true, "Profile picture deleted successfully", null);
        }
    }
} 