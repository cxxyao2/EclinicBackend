using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using EclinicBackend.Enums;
using System.Text.Json.Serialization;

namespace EclinicBackend.Models;

[Index(nameof(Email), IsUnique = true)]
public class User : AuditableEntity
{
    [Key]
    [Column("user_id")]
    public int UserID { get; set; }


    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("user_name")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [JsonIgnore]
    [Column("password_hash")]
    public byte[] PasswordHash { get; set; } = null!;

    [JsonIgnore]
    [Column("password_salt")]
    public byte[] PasswordSalt { get; set; } = null!;

    [Column("role")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; } = UserRole.Nurse;  //   Admin, Practitioner, etc.

    [Column("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
    [Column("token_created")]
    public DateTime TokenCreated { get; set; }
    [Column("token_expires")]
    public DateTime TokenExpires { get; set; }

    [Column("practitioner_id")]
    public int? PractitionerId { get; set; }

    public Practitioner? Practitioner { get; set; }

    [Column("activation_code")]
    public string? ActivationCode { get; set; }

    [Column("is_activated")]
    public bool IsActivated { get; set; } = false;

    [Column("activation_code_expires")]
    public DateTime? ActivationCodeExpires { get; set; }

    [Column("password_reset_token")]
    public string? PasswordResetToken { get; set; }

    [Column("password_reset_token_expires")]
    public DateTime? PasswordResetTokenExpires { get; set; }
}

