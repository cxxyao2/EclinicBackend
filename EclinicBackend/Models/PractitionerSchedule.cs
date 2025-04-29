using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EclinicBackend.Models
{
    public class PractitionerSchedule : AuditableEntity
    {
        [Key]
        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Required]
        [Column("practitioner_id")]
        public int PractitionerId { get; set; }

        [Required]
        [Column("day_of_week")]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        [Column("is_available")]
        public bool IsAvailable { get; set; } = true;


        [Column("patient_id")]
        public int? PatientId { get; set; }

        [MaxLength(255)]
        [Column("reason_for_visit")]
        public string? ReasonForVisit { get; set; }

        [Column("start_date_time")]
        public DateTime StartDateTime { get; set; }

        [Column("end_date_time")]
        public DateTime EndDateTime { get; set; }

        public Patient? Patient { get; set; }

        public Practitioner? Practitioner { get; set; }
    }
}
