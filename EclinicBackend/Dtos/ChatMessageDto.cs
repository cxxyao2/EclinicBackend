namespace EclinicBackend.Dtos
{
    public class ChatMessageDto
    {
        public int MessageId { get; set; }
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}