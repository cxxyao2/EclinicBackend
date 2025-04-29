using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EclinicBackend.Models
{
    public class ChatRoomParticipant
    {
        [Key]
        [Column("participant_id")]
        public int ParticipantId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        [Column("chat_room_id")]
        public int ChatRoomId { get; set; }

        public ChatRoom ChatRoom { get; set; } = null!;

        [Column("joined_at")]
        public DateTime JoinedAt { get; set; }

        [Column("left_at")]
        public DateTime? LeftAt { get; set; }
    }
}
