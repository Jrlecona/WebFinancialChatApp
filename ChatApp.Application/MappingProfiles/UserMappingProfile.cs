using AutoMapper;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}