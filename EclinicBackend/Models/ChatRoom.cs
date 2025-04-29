using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EclinicBackend.Models
{
    public class ChatRoom : AuditableEntity
    {
        [Key]
        [Column("chat_room_id")]
        public int ChatRoomId { get; set; }

        [Required]
        [Column("patient_id")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column("topic")]
        public string Topic { get; set; } = string.Empty;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<ChatRoomParticipant> Participants { get; set; } = new List<ChatRoomParticipant>();
    }
}
