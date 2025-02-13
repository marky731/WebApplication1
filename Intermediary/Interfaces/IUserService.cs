using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace Intermediary.Interfaces;

public interface IUserService 
{
    ApiResponse<List<UserDto>> GetAllUsers(int pageNumber, int pageSize);
    ApiResponse<UserDto> GetUserById(int id);
    ApiResponse<UserToAddDto> CreateUser(UserToAddDto userToAddDto);
    ApiResponse<UserDto> UpdateUser(UserDto userDto); 
    ApiResponse<string?> DeleteUser(int id);
    
    
}