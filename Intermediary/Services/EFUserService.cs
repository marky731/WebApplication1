using AutoMapper;
using Intermediary.Interfaces;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using EntityLayer.Models;

namespace Intermediary.Services;

public class EfUserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public EfUserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<UserDto>>> GetAllUsers(int pageNumber, int pageSize)
    {
        var users = await _userRepository.GetAllAsync(pageNumber, pageSize);
        var userDtos = _mapper.Map<List<UserDto>>(users);
        return new ApiResponse<List<UserDto>>(true, "Users retrieved successfully", userDtos);
    }

    public async Task<ApiResponse<UserDto>> GetUserById(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        var userDto = _mapper.Map<UserDto>(user);
        return new ApiResponse<UserDto>(true, "User retrieved successfully", userDto);
    }

    public async Task<ApiResponse<UserToAddDto>> CreateUser(UserToAddDto userToAddDto)
    {
        var user = _mapper.Map<User>(userToAddDto);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return new ApiResponse<UserToAddDto>(true, "User added successfully", userToAddDto);
    }

    public async Task<ApiResponse<UserDto>> UpdateUser(UserDto userDto)
    {
        var user = await _userRepository.GetByIdAsync(userDto.Id);
        if (user == null)
            throw new Exception("User not found");

        _mapper.Map(userDto, user);
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
        return new ApiResponse<UserDto>(true, "User updated successfully", userDto);
    }

    public async Task<ApiResponse<string?>> DeleteUser(int userId)
    {
        await _userRepository.DeleteAsync(userId);
        await _userRepository.SaveChangesAsync();
        return new ApiResponse<string?>(true, "User deleted successfully", null);
    }
    
}