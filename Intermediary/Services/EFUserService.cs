using AutoMapper;
using Intermediary.Interfaces;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using EntityLayer.Models;

namespace Intermediary.Services;

public class EfUserService : IUserService
{
    private readonly IUserRepository _userRepository; // Inject IUserRepository
    private readonly IMapper _mapper;

    public EfUserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public ApiResponse<List<UserDto>> GetAllUsers(int pageNumber, int pageSize)
    {
        var users = _userRepository.GetAllUsers(pageNumber, pageSize);
        var userDtos = _mapper.Map<List<UserDto>>(users);

        return new ApiResponse<List<UserDto>>(true, "Users retrieved successfully", userDtos);
    }

    public ApiResponse<UserToAddDto> CreateUser(UserToAddDto userToAddDto)
    {
        var user = _mapper.Map<User>(userToAddDto);
        _userRepository.AddUser(user);  // Call repository method

        return new ApiResponse<UserToAddDto>(true, "User added successfully", userToAddDto);
    }

    public ApiResponse<UserDto> UpdateUser(UserDto userDto)
    {
        var user = _userRepository.GetUserById(userDto.Id);  // Call repository method
        if (user == null)
        {
            throw new Exception("User not found");
        }

        _mapper.Map(userDto, user);
        _userRepository.UpdateUser(user);  // Call repository method

        return new ApiResponse<UserDto>(true, "User updated successfully", userDto);
    }

    public ApiResponse<string?> DeleteUser(int userId)
    {
        var user = _userRepository.GetUserById(userId);  // Call repository method
        if (user == null)
        {
            throw new Exception("User not found");
        }

        _userRepository.DeleteUser(userId);  // Call repository method

        return new ApiResponse<string?>(true, "User deleted successfully", null);
    }

    public ApiResponse<UserDto> GetUserById(int userId)
    {
        var user = _userRepository.GetUserById(userId);  // Call repository method
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var userDto = _mapper.Map<UserDto>(user);
        return new ApiResponse<UserDto>(true, "User retrieved successfully", userDto);
    }
}