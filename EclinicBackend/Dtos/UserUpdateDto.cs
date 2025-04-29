using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EclinicBackend.Enums;

namespace EclinicBackend.Dtos
{
    public class UserUpdateDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(6)]
        [MaxLength(30)]
        public string? Password { get; set; }

        public int? PractitionerId { get; set; }
    }
}
