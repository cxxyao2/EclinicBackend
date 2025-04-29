using EclinicBackend.Models;

namespace EclinicBackend.Dtos
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
    }


    public record TokenPair
    {
        public string AccessToken { get; set; } = string.Empty;
        public RefreshToken RefreshToken = new();
        public User user = null!;
    }
}
