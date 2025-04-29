using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EclinicBackend.Models
{
    public class ChatMessage : AuditableEntity
    {
        [Key]
        [Column("message_id")]
        public int MessageId { get; set; }

        [Required]
        [Column("sender_id")]
        public int SenderId { get; set; }


        public User Sender { get; set; } = null!;

        [Required]
        [Column("chat_room_id")]
        public int ChatRoomId { get; set; }


        public ChatRoom ChatRoom { get; set; } = null!;

        [Required]
        [MaxLength(2000)]
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
