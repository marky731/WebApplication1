using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace Intermediary.Interfaces;

public interface IProfilePicService
{
    Task<ApiResponse<List<ProfilePicDto>>> GetAllProfilePics(int pageNumber, int pageSize);
    Task<ApiResponse<ProfilePicDto>> GetProfilePicById(int id);
    Task<ApiResponse<ProfilePicDto>> CreateProfilePic(ProfilePicDto profilePicDto);
    Task<ApiResponse<ProfilePicDto>> UpdateProfilePic(ProfilePicDto profilePicDto);
    Task<ApiResponse<string?>> DeleteProfilePic(int id);
} 