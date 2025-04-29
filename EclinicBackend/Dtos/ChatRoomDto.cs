using System.ComponentModel.DataAnnotations;

namespace EclinicBackend.Dtos
{
    public class ChatRoomDto
    {
        [Required]
        public int ChatRoomId { get; set; }

        [Required]
        public int PatientId { get; set; }

        public string Topic { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
