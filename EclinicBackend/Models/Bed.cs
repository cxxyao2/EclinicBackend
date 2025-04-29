using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EclinicBackend.Models
{
    public class Bed : AuditableEntity
    {
        [Key]
        [Column("bed_id")]
        public int BedId { get; set; }

        [Column("room_number")]
        public string RoomNumber { get; set; } = string.Empty; // Room number

        [Column("bed_number")]
        public string BedNumber { get; set; } = string.Empty; // Bed number

        [Column("inpatient_id")]
        public int? InpatientId { get; set; } // Nullable: NULL if no patient is occupying

        public Inpatient? Inpatient { get; set; } // Navigation property for patient record
    }
}
