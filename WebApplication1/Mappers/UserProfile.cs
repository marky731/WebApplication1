using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserToAddDto>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
    }
}