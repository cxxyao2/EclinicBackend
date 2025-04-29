using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services
{
    public interface IChatService
    {
        Task<ServiceResponse<ChatRoomDto>> CreateRoom(CreateChatRoomDto request);
        Task<ServiceResponse<List<ChatRoomDto>>> GetUserRooms();
        Task<ServiceResponse<List<ChatMessageDto>>> GetRoomMessages(int roomId);
        Task<ServiceResponse<bool>> AddParticipant(int roomId, int userId);

        // New methods for real-time chat
        Task<ServiceResponse<ChatMessageDto>> SaveMessage(ChatMessageDto message);
        Task<ServiceResponse<bool>> RemoveParticipant(int roomId, int userId);
    }
}
