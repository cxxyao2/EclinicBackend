using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EclinicBackend.Enums;

namespace EclinicBackend.Models;

// depracated. 2024. Dec.
public class PractitionerAvailability : AuditableEntity
{
    [Key]
    [Column("available_id")]
    public int AvailableId { get; set; }
    [Required]
    [Column("practitioner_id")]
    public int PractitionerId { get; set; }
    [Column("slot_date_time")]
    public DateTime SlotDateTime { get; set; } // Stores date, time, and timezone offset
    [Column("is_available")]
    public bool IsAvailable { get; set; } // true for available, false for unavailable


    public Practitioner? Practitioner { get; set; }

}
