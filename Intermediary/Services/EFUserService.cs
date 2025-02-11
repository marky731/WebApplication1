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

    public ApiResponse<List<UserDto>> GetAllUsers()
    {
        var users = _userRepository.GetAllUsers();  // Call repository method
        var userDtos = _mapper.Map<List<UserDto>>(users);

        return new ApiResponse<List<UserDto>>(true, "Users retrieved successfully", userDtos);
    }

    public ApiResponse<UserDto> CreateUser(UserDto userDto)
    {
        var user = _mapper.Map<user>(userDto);
        _userRepository.AddUser(user);  // Call repository method

        return new ApiResponse<UserDto>(true, "user added successfully", userDto);
    }

    public ApiResponse<UserDto> UpdateUser(UserDto userDto)
    {
        var user = _userRepository.GetUserById(userDto.Id);  // Call repository method
        if (user == null)
        {
            throw new Exception("user not found");
        }

        _mapper.Map(userDto, user);
        _userRepository.UpdateUser(user);  // Call repository method

        return new ApiResponse<UserDto>(true, "user updated successfully", userDto);
    }

    public ApiResponse<string?> DeleteUser(int userId)
    {
        var user = _userRepository.GetUserById(userId);  // Call repository method
        if (user == null)
        {
            throw new Exception("user not found");
        }

        _userRepository.DeleteUser(userId);  // Call repository method

        return new ApiResponse<string?>(true, "user deleted successfully", null);
    }

    public ApiResponse<UserDto> GetUserById(int userId)
    {
        var user = _userRepository.GetUserById(userId);  // Call repository method
        if (user == null)
        {
            throw new Exception("user not found");
        }

        var userDto = _mapper.Map<UserDto>(user);
        return new ApiResponse<UserDto>(true, "user retrieved successfully", userDto);
    }
}