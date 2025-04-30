using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EclinicBackend.Services;
using EclinicBackend.Dtos;

namespace EclinicBackend.Controllers
{
    [Authorize(Policy = "MedicalStaff")]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("rooms")]
        public async Task<ActionResult<ServiceResponse<ChatRoomDto>>> CreateRoom([FromBody] CreateChatRoomDto request)
        {
            return Ok(await _chatService.CreateRoom(request));
        }

        [HttpGet("rooms")]
        public async Task<ActionResult<ServiceResponse<List<ChatRoomDto>>>> GetRooms()
        {
            return Ok(await _chatService.GetUserRooms());
        }

        [HttpGet("rooms/{roomId}/messages")]
        public async Task<ActionResult<ServiceResponse<List<ChatMessageDto>>>> GetMessages(int roomId)
        {
            return Ok(await _chatService.GetRoomMessages(roomId));
        }

        [HttpPost("rooms/{roomId}/participants")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddParticipant(int roomId, [FromBody] AddParticipantDto request)
        {
            return Ok(await _chatService.AddParticipant(roomId, request.UserId));
        }
    }
}