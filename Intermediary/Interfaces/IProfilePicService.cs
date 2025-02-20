using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace Intermediary.Interfaces;

public interface IProfilePicService
{
    Task<ApiResponse<List<ImageDto>>> GetAllProfilePics(int pageNumber, int pageSize);
    Task<ApiResponse<ImageDto>> GetProfilePicById(int id);
    Task<ApiResponse<ImageUploadDto?>> CreateProfilePic(ImageUploadDto imageDto);
    Task<ApiResponse<ImageDto>> UpdateProfilePic(ImageDto imageDto);
    Task<ApiResponse<string?>> DeleteProfilePic(int id);
} 