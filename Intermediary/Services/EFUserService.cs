using AutoMapper;
using Intermediary.Interfaces;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using EntityLayer.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Intermediary.Services;

public class EfUserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<UserDto> _userValidator;
    private readonly IValidator<UserToAddDto> _userToAddValidator;

    public EfUserService(
        IUserRepository userRepository,
        IMapper mapper,
        IValidator<UserDto> userValidator,
        IValidator<UserToAddDto> userToAddValidator)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _userValidator = userValidator;
        _userToAddValidator = userToAddValidator;
    }

    public async Task<ApiResponse<List<UserDto>>> GetAllUsers(int pageNumber, int pageSize)
    {
        var users = await _userRepository.GetAllAsync(pageNumber, pageSize);
        var totalCount = await _userRepository.GetTotalCountAsync();
        var userDtos = _mapper.Map<List<UserDto>>(users);
        return new ApiResponse<List<UserDto>>(true, "Users retrieved successfully", userDtos, pageNumber, pageSize, totalCount);
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
        ValidationResult validationResult = await _userToAddValidator.ValidateAsync(userToAddDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = _mapper.Map<User>(userToAddDto);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return new ApiResponse<UserToAddDto>(true, "User added successfully", userToAddDto);
    }

    public async Task<ApiResponse<UserDto>> UpdateUser(UserDto userDto)
    {
        ValidationResult validationResult = await _userValidator.ValidateAsync(userDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

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