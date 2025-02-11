// using AutoMapper;
// using Microsoft.Extensions.Configuration;
// using EntityLayer.Dtos;
// using EntityLayer.Interfaces;
// using EntityLayer.Models;
//
// public class UserService : IUserService
// {
//     PostgreManager _context;
//     private readonly IMapper _mapper;
//
//     public UserService(IConfiguration config, IMapper mapper)
//     {
//         _context = new PostgreManager(config);
//         _mapper = mapper;
//     }
//
//     public ApiResponse<List<UserDto>> GetAllUsers() // Dto donuyor hepsi
//     {
//         string sql = @"
//             SELECT *
//             FROM TutorialAppSchema.Users ORDER BY id ASC";
//         IEnumerable<user> users = _context.LoadData<user>(sql);
//         List<UserDto> userDtos = _mapper.Map<List<UserDto>>(users);
//
//         return new ApiResponse<List<UserDto>>(true, "Users retrieved successfully", userDtos);
//     }
//
//
//     public ApiResponse<string> CreateUser(UserDto userDto)
//     {
//         var user = _mapper.Map<user>(userDto);
//
//         string sql = @"
//             INSERT INTO TutorialAppSchema.Users(
//                 FirstName,
//                 LastName,
//                 Email,
//                 Gender,
//                 Active
//             ) VALUES (" +
//                      "'" + user.FirstName +
//                      "', '" + user.LastName +
//                      "', '" + user.Email +
//                      "', '" + user.Gender +
//                      "', '" + user.Active +
//                      "')";
//
//         Console.WriteLine(sql);
//
//         if (_context.ExecuteSql(sql))
//         {
//             return new ApiResponse<string>(true, "user added successfully", null);
//         }
//
//         throw new Exception("Failed to Add user");
//     }
//
//     public ApiResponse<UserDto> UpdateUser(UserDto userDto)
//     {
//         string sql = @"
//         UPDATE TutorialAppSchema.Users
//             SET FirstName = '" + userDto.FirstName +
//                      "', LastName = '" + userDto.LastName +
//                      "', Email = '" + userDto.Email +
//                      "', Gender = '" + userDto.Gender +
//                      "', Active = '" + userDto.Active +
//                      "' WHERE id = " + userDto.Id;
//
//         Console.WriteLine(sql);
//
//         if (_context.ExecuteSql(sql))
//         {
//             return new ApiResponse<UserDto>(true, "user updated successfully", userDto);
//         }
//
//         throw new Exception("Failed to Update user");
//     }
//
//     public ApiResponse<string> DeleteUser(int userId)
//     {
//         string sql = @"
//             DELETE FROM TutorialAppSchema.Users 
//                 WHERE id = " + userId.ToString();
//
//         Console.WriteLine(sql);
//
//         if (_context.ExecuteSql(sql))
//         {
//             return new ApiResponse<string>(true, "user deleted successfully", null);
//         }
//
//         throw new Exception("Failed to Delete user");
//     }
//
//
//     public ApiResponse<UserDto> GetUserById(int userId)
//     {
//         string sql = @"
//             SELECT *
//             FROM TutorialAppSchema.Users
//                 WHERE id = " + userId.ToString(); //"7"
//         user user = _context.LoadDataSingle<user>(sql);
//         UserDto userDtos = _mapper.Map<UserDto>(user);
//         return new ApiResponse<UserDto>(true, "user retrieved successfully", userDtos);
//     }
// }