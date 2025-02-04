using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Interfaces;

public interface IUserService 
{
    ApiResponse<List<UserDto>> GetAllUsers();
    ApiResponse<UserDto> GetUserById(int id);
    ApiResponse<string> CreateUser(UserDto userDto);
    ApiResponse<UserDto> UpdateUser(UserDto userDto); 
    ApiResponse<string> DeleteUser(int id);
    
    
}