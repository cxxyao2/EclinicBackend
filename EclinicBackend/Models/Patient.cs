using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EclinicBackend.Models
{
    public class Patient : AuditableEntity
    {
        [Key]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("first_name")]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("last_name")]
        public required string LastName { get; set; }

        [Required]
        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(10)]
        [Column("gender")]
        public string? Gender { get; set; }

        [MaxLength(255)]
        [Column("address")]
        public string? Address { get; set; }

        [MaxLength(20)]
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        [Column("email")]
        public string? Email { get; set; }

        [MaxLength(100)]
        [Column("emergency_contact")]
        public string? EmergencyContact { get; set; }

        [MaxLength(20)]
        [Column("emergency_phone_number")]
        public string? EmergencyPhoneNumber { get; set; }


    }
}
