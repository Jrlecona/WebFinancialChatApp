using AutoMapper;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.MappingProfiles;

public class ChatRoomMappingProfile : Profile
{
    public ChatRoomMappingProfile()
    {
        CreateMap<ChatRoomDTO, ChatRoom>().ReverseMap();
    }
}