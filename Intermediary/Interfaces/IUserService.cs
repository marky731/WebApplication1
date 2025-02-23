using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace Intermediary.Interfaces;

public interface IUserService
{
    Task<ApiResponse<List<UserDto>>> GetAllUsers(int pageNumber, int pageSize);
    Task<ApiResponse<UserDto>> GetUserById(int id);
    Task<ApiResponse<UserToAddDto>> CreateUser(UserToAddDto userToAddDto);
    Task<ApiResponse<UserDto>> UpdateUser(UserDto userDto);
    Task<ApiResponse<string?>> DeleteUser(int id);
    Task<ApiResponse<string>> RegisterUser(UserRegisterDto userRegisterDto); // Add register method
    Task<ApiResponse<string>> LoginUser(UserLoginDto userLoginDto); // Add login method
    Task<ApiResponse<UserInfoDto>> GetUserInfoById(int userId);
}