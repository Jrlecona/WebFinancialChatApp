using AutoMapper;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.MappingProfiles;

public class MessageMappingProfile : Profile
{
    public MessageMappingProfile()
    {
        CreateMap<MessageDTO, Message>().ReverseMap();
    }
}