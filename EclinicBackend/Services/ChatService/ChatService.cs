using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EclinicBackend.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<ChatRoomDto>> CreateRoom(CreateChatRoomDto request)
        {
            var response = new ServiceResponse<ChatRoomDto>();
            try
            {
                var chatRoom = new ChatRoom
                {
                    PatientId = request.PatientId,
                    Topic = request.Topic,
                    CreatedBy = GetUserId(),
                    CreatedAt = DateTime.UtcNow
                };

                _context.ChatRooms.Add(chatRoom);
                await _context.SaveChangesAsync();

                // Add creator as participant
                var participant = new ChatRoomParticipant
                {
                    ChatRoomId = chatRoom.ChatRoomId,
                    UserId = GetUserId(),
                    JoinedAt = DateTime.UtcNow
                };
                _context.ChatRoomParticipants.Add(participant);
                await _context.SaveChangesAsync();

                response.Data = new ChatRoomDto
                {
                    ChatRoomId = chatRoom.ChatRoomId,
                    PatientId = chatRoom.PatientId,
                    Topic = chatRoom.Topic,
                    CreatedAt = chatRoom.CreatedAt,

                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<ChatRoomDto>>> GetUserRooms()
        {
            var response = new ServiceResponse<List<ChatRoomDto>>();
            try
            {
                var userId = GetUserId();
                var rooms = await _context.ChatRooms
                    .Include(r => r.Participants)
                    .Where(r => r.Participants.Any(p => p.UserId == userId))
                    .Select(r => new ChatRoomDto
                    {
                        ChatRoomId = r.ChatRoomId,
                        PatientId = r.PatientId,
                        Topic = r.Topic,
                        CreatedAt = r.CreatedAt
                    })
                    .ToListAsync();

                response.Data = rooms;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<ChatMessageDto>>> GetRoomMessages(int roomId)
        {
            var response = new ServiceResponse<List<ChatMessageDto>>();
            try
            {
                var messages = await _context.ChatMessages
                    .Where(m => m.ChatRoomId == roomId)
                    .OrderBy(m => m.CreatedAt)
                    .Select(m => new ChatMessageDto
                    {
                        MessageId = m.MessageId,
                        ChatRoomId = m.ChatRoomId,
                        SenderId = m.CreatedBy,
                        Content = m.Content,
                        CreatedAt = m.CreatedAt
                    })
                    .ToListAsync();

                response.Data = messages;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> AddParticipant(int roomId, int userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                // Check if participant already exists
                var existingParticipant = await _context.ChatRoomParticipants
                    .FirstOrDefaultAsync(p => p.ChatRoomId == roomId && p.UserId == userId);

                if (existingParticipant != null)
                {
                    // If exists but previously left, update JoinedAt and clear LeftAt
                    if (existingParticipant.LeftAt.HasValue)
                    {
                        existingParticipant.JoinedAt = DateTime.UtcNow;
                        existingParticipant.LeftAt = null;
                        await _context.SaveChangesAsync();
                    }
                    // If exists and hasn't left, do nothing
                }
                else
                {
                    // If doesn't exist, create new participant
                    var participant = new ChatRoomParticipant
                    {
                        ChatRoomId = roomId,
                        UserId = userId,
                        JoinedAt = DateTime.UtcNow
                    };
                    _context.ChatRoomParticipants.Add(participant);
                    await _context.SaveChangesAsync();
                }

                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<ChatMessageDto>> SaveMessage(ChatMessageDto messageDto)
        {
            var response = new ServiceResponse<ChatMessageDto>();
            try
            {
                var message = new ChatMessage
                {
                    ChatRoomId = messageDto.ChatRoomId,
                    Content = messageDto.Content,
                    SenderId = messageDto.SenderId,
                    CreatedBy = messageDto.SenderId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ChatMessages.Add(message);
                await _context.SaveChangesAsync();

                messageDto.MessageId = message.MessageId;
                response.Data = messageDto;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveParticipant(int roomId, int userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var participant = await _context.ChatRoomParticipants
                    .FirstOrDefaultAsync(p => p.ChatRoomId == roomId && p.UserId == userId);

                if (participant != null)
                {
                    participant.LeftAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}

