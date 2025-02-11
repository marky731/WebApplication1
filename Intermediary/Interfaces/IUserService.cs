using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace Intermediary.Interfaces;

public interface IUserService 
{
    ApiResponse<List<UserDto>> GetAllUsers();
    ApiResponse<UserDto> GetUserById(int id);
    ApiResponse<UserDto> CreateUser(UserDto userDto);
    ApiResponse<UserDto> UpdateUser(UserDto userDto); 
    ApiResponse<string?> DeleteUser(int id);
    
    
}