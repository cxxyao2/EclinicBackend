using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EclinicBackend.Models
{
    public class Medication : AuditableEntity
    {
        [Key]
        [Column("medication_id")]
        public int MedicationId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public required string Name { get; set; }

        [MaxLength(100)]
        [Column("dosage")]
        public string? Dosage { get; set; }

        [MaxLength(50)]
        [Column("route")]
        public string? Route { get; set; } // Oral, IV, etc.

        [MaxLength(100)]
        [Column("frequency")]
        public string? Frequency { get; set; }

        [MaxLength(500)]
        [Column("side_effects")]
        public string? SideEffects { get; set; }

    }
}
