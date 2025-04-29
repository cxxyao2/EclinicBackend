using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EclinicBackend.Models;
public class Prescription : AuditableEntity
{
    [Key]
    [Column("prescription_id")]
    public int PrescriptionId { get; set; }

    [Required]
    [Column("visit_id")]
    public int VisitId { get; set; }

    [Required]
    [Column("patient_id")]
    public int PatientId { get; set; }

    [Required]
    [Column("practitioner_id")]
    public int PractitionerId { get; set; }


    [Required]
    [Column("medication_id")]
    public int MedicationId { get; set; }

    [MaxLength(100)]
    [Column("dosage")]
    public string? Dosage { get; set; }

    [Column("start_date")]
    public DateTime? StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [MaxLength(500)]
    [Column("notes")]
    public string? Notes { get; set; }




    public VisitRecord? VisitRecord { get; set; }
    public Patient? Patient { get; set; }


    public Practitioner? Practitioner { get; set; }


    public Medication? Medication { get; set; }
}


