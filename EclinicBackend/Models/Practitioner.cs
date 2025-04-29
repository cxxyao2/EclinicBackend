using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EclinicBackend.Models
{
    public class Practitioner : AuditableEntity
    {
        [Key]
        [Column("practitioner_id")]
        public int PractitionerId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("first_name")]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("last_name")]
        public required string LastName { get; set; }

        [MaxLength(100)]
        [Column("specialty")]
        public string? Specialty { get; set; }

        [MaxLength(20)]
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        [Column("email")]
        public string? Email { get; set; }


    }
}
