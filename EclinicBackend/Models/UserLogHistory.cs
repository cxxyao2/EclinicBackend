using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EclinicBackend.Models
{
    public class UserLogHistory : AuditableEntity
    {
        [Key]
        [Column("log_id")]
        public int LogId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("user_name")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [Column("ip_address")]
        public string IpAddress { get; set; } = string.Empty;

        [Required]
        [Column("login_time")]
        public DateTime LoginTime { get; set; }

        [Column("logout_time")]
        public DateTime? LogoutTime { get; set; }

        public User User { get; set; } = null!;
    }
}