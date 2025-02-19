using AutoMapper;
using EntityLayer.Dtos;
using EntityLayer.Models;

namespace Intermediary.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserToAddDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<ProfilePic, ProfilePicDto>().ReverseMap();
        }
    }
}