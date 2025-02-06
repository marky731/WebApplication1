using AutoMapper;
using Presentation.Dtos;
using Presentation.Models;

namespace SecondLayer.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // CreateMap<User, UserToAddDto>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
    }
}