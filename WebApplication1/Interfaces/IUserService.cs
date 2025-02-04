using WebApplication1.Dtos;


namespace WebApplication1.Interfaces;

public interface IUserService 
{
    ApiResponse<List<UserDto>> GetAllUsers();
    ApiResponse<UserDto> GetUserById(int id);
    ApiResponse<UserDto> CreateUser(UserDto userDto);
    ApiResponse<UserDto> UpdateUser(UserDto userDto); 
    ApiResponse<string> DeleteUser(int id);
    
    
}