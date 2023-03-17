using ChatApp.Application.DTOs;

namespace ChatApp.Application.Interfaces;

public interface IMessageService
{
    Task<List<MessageDTO>> GetLast50Async(Guid chatRoomId);
    Task<List<MessageDTO>> GetBySenderIdAsync(Guid senderId, Guid chatRoomId);
    Task AddAsync(MessageDTO message);
}