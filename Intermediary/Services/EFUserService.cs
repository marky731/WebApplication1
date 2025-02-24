using AutoMapper;
using Intermediary.Interfaces;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using EntityLayer.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Intermediary.Services;

public class EfUserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<UserDto> _userValidator;
    private readonly IValidator<UserToAddDto> _userToAddValidator;
    private readonly IValidator<UserRegisterDto> _userRegisterValidator; 
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IRoleRepository _roleRepository;
    private readonly IJwtService _jwtService;


    public EfUserService(
        IUserRepository userRepository,
        IMapper mapper,
        IValidator<UserDto> userValidator,
        IValidator<UserToAddDto> userToAddValidator,
        IValidator<UserRegisterDto> userRegisterValidator, 
        IPasswordHasher<User> passwordHasher,
        IRoleRepository roleRepository,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _userValidator = userValidator;
        _userToAddValidator = userToAddValidator;
        _userRegisterValidator = userRegisterValidator; 
        _passwordHasher = passwordHasher;
        _roleRepository = roleRepository;
        _jwtService = jwtService;
    }
    
    public async Task<ApiResponse<PaginatedResponse<List<UserDto>>>> GetAllUsers(int pageNumber, int pageSize)
    {
        var users = await _userRepository.GetAllPaginatedAsync(pageNumber, pageSize);
        var totalCount = await _userRepository.GetTotalCountAsync();
        var userDtos = _mapper.Map<List<UserDto>>(users);
        PaginatedResponse<List<UserDto>> paginatedResponse = new PaginatedResponse<List<UserDto>>(userDtos, pageNumber, pageSize, totalCount);
        return new ApiResponse<PaginatedResponse<List<UserDto>>>(true, "Users retrieved successfully", paginatedResponse);
    }

    public async Task<ApiResponse<UserDto>> GetUserById(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        var userDto = _mapper.Map<UserDto>(user);
        return new ApiResponse<UserDto>(true, "User retrieved successfully", userDto);
    }
    
    public async Task<ApiResponse<UserInfoDto>> GetUserInfoById(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        var userDto = _mapper.Map<UserInfoDto>(user);
        userDto.Role = user.Role.Name;
        return new ApiResponse<UserInfoDto>(true, "User retrieved successfully", userDto);
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

        var existingUser = await _userRepository.GetByIdAsync(userDto.Id);
        if (existingUser == null)
            throw new Exception("User not found");

        var userToUpdate = new User
        {
            Id = userDto.Id,
            Firstname = userDto.Firstname,
            Surname = userDto.Surname,
            Gender = userDto.Gender,
            RoleId = userDto.RoleId,
            Addresses = userDto.Addresses != null
                ? _mapper.Map<ICollection<Address>>(userDto.Addresses)
                : null
        };

        await _userRepository.UpdateAsync(userToUpdate);
        await _userRepository.SaveChangesAsync();

        var updatedUser = await _userRepository.GetByIdAsync(userDto.Id);
        var updatedUserDto = _mapper.Map<UserDto>(updatedUser);

        return new ApiResponse<UserDto>(true, "User updated successfully", updatedUserDto);
    }

    public async Task<ApiResponse<string?>> DeleteUser(int userId)
    {
        await _userRepository.DeleteAsync(userId);
        await _userRepository.SaveChangesAsync();
        return new ApiResponse<string?>(true, "User deleted successfully", null);
    }
    
    public async Task<ApiResponse<string>> RegisterUser(UserRegisterDto userRegisterDto)
    {
        ValidationResult validationResult = await _userRegisterValidator.ValidateAsync(userRegisterDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var existingUser = await _userRepository.GetByEmailAsync(userRegisterDto.Email);
        if (existingUser != null)
        {
            throw new Exception("User with this email already exists.");
        }
        
        var role = await _roleRepository.GetByIdAsync(userRegisterDto.RoleId);
        if (role == null)
        {
            throw new Exception("Invalid role selected.");
        }

        var user = new User
        {
            Firstname = userRegisterDto.Firstname,
            Surname = userRegisterDto.Surname,
            Gender = userRegisterDto.Gender,
            RoleId = userRegisterDto.RoleId,
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, userRegisterDto.Password);
        user.Email = userRegisterDto.Email;

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user.Id, user.Email, role.Name);

        return new ApiResponse<string>(true, "User registered successfully", token);
    }
    
    public async Task<ApiResponse<string>> LoginUser(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);
        if (user == null)
        {
            throw new Exception("Invalid email or password.");
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDto.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new Exception("Invalid email or password.");
        }

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.Name);

        return new ApiResponse<string>(true, "Login successful", token);
    }
}