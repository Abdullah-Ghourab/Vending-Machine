using AutoMapper;
using FlapKap.Core.DTOs;
using FlapKap.Core.Models;

namespace FlapKap.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<RegisterDto, User>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
