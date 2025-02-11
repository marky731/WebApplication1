using AutoMapper;
using EntityLayer.Dtos;
using EntityLayer.Models;

namespace Intermediary.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // CreateMap<user, UserToAddDto>().ReverseMap();
        CreateMap<UserDto, user>().ReverseMap();
    }
}