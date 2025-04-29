using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using EclinicBackend.Data;
using EclinicBackend.Services;
using EclinicBackend.Dtos;
using System.Security.Claims;
using EclinicBackend.Attributes;

namespace EclinicBackend.Hubs
{
    [SignalRAuth]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        private int GetUserId()
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new HubException("User not authenticated");
            }
            return int.Parse(userIdClaim);
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                // Verify user is authenticated when connecting
                _ = GetUserId();
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                throw new HubException("Authentication failed." + ex.Message);
            }
        }

        public async Task SendMessage(int chatRoomId, string message)
        {
            var userId = GetUserId();
            var messageDto = new ChatMessageDto
            {
                ChatRoomId = chatRoomId,
                Content = message,
                SenderId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _chatService.SaveMessage(messageDto);
            await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", messageDto);
        }

        public async Task JoinRoom(int chatRoomId)
        {
            var userId = GetUserId();
            await _chatService.AddParticipant(chatRoomId, userId);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
        }

        public async Task LeaveRoom(int chatRoomId)
        {
            var userId = GetUserId();
            await _chatService.RemoveParticipant(chatRoomId, userId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
        }
    }
}

