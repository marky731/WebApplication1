using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace Intermediary.Interfaces;

public interface IProfilePicService
{
    Task<ApiResponse<List<ImageDto>>> GetAllProfilePics(int pageNumber, int pageSize);
    Task<ApiResponse<ImageDto>> GetProfilePicById(int id);
    Task<ApiResponse<ImageDto>> CreateProfilePic(ImageDto imageDto);
    Task<ApiResponse<ImageDto>> UpdateProfilePic(ImageDto imageDto);
    Task<ApiResponse<string?>> DeleteProfilePic(int id);
} 