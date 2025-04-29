using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EclinicBackend.Enums;


namespace EclinicBackend.Dtos
{
    public class UserCreateDto
    {

        [JsonPropertyName("name")]
        public string UserName { get; set; } = string.Empty;


        public UserRole? Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [MaxLength(30)]
        public string Password { get; set; } = string.Empty;


    }
}