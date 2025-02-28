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
            CreateMap<User, UserInfoDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Image, ImageDto>().ReverseMap();
        }
    }
}