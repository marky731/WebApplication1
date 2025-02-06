using AutoMapper;
using Presentation.Data;
using Presentation.Dtos;
using Presentation.Interfaces;
using Presentation.Models;

namespace SecondLayer.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ApiResponse<List<UserDto>> GetAllUsers()
    {
        var users = _context.users.OrderBy(u => u.Id).ToList();
        var userDtos = _mapper.Map<List<UserDto>>(users);

        return new ApiResponse<List<UserDto>>(true, "Users retrieved successfully", userDtos);
    }

    public ApiResponse<UserDto> CreateUser(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        _context.users.Add(user);
        _context.SaveChanges();

        return new ApiResponse<UserDto>(true, "User added successfully", userDto);
    }

    public ApiResponse<UserDto> UpdateUser(UserDto userDto)
    {
        var user = _context.users.Find(userDto.Id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        _mapper.Map(userDto, user);
        _context.SaveChanges();

        return new ApiResponse<UserDto>(true, "User updated successfully", userDto);
    }

    public ApiResponse<string> DeleteUser(int userId)
    {
        var user = _context.users.Find(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        _context.users.Remove(user);
        _context.SaveChanges();

        return new ApiResponse<string>(true, "User deleted successfully", null);
    }

    public ApiResponse<UserDto> GetUserById(int userId)
    {
        var user = _context.users.Find(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var userDto = _mapper.Map<UserDto>(user);
        return new ApiResponse<UserDto>(true, "User retrieved successfully", userDto);
    }
}