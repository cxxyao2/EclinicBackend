using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EclinicBackend.Models;
public class VisitRecord : AuditableEntity
{
    [Key]
    [Column("visit_id")]
    public int VisitId { get; set; }

    [Required]
    [Column("patient_id")]
    public int PatientId { get; set; }

    [Required]
    [Column("practitioner_id")]
    public int PractitionerId { get; set; }

    [Column("schedule_id")]
    public int ScheduleId { get; set; }

    [Column("practitioner_signature_path")]
    public string PractitionerSignaturePath { get; set; } = "";

    [Required]
    [Column("visit_date")]
    public DateTime VisitDate { get; set; }

    [MaxLength(500)]
    [Column("diagnosis")]
    public string Diagnosis { get; set; } = "";

    [MaxLength(500)]
    [Column("treatment")]
    public string Treatment { get; set; } = "";

    [MaxLength(1000)]
    [Column("notes")]
    public string Notes { get; set; } = "";


    public Patient? Patient { get; set; }


    public Practitioner? Practitioner { get; set; }
}

