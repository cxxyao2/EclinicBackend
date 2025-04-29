using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EclinicBackend.Models;
public class LabTest : AuditableEntity
{
    [Key]
    [Column("lab_test_id")]
    public int LabTestId { get; set; }

    [Required]
    [Column("test_name")]
    public string TestName { get; set; } = string.Empty;

    [Required]
    [Column("patient_id")]
    public int PatientId { get; set; }

    [Required]
    [Column("practitioner_id")]
    public int PractitionerId { get; set; }

    [Column("visit_id")]
    public int? VisitId { get; set; }

    [Required]
    [Column("test_date")]
    public DateTime TestDate { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("test_type")]
    public string TestType { get; set; } = string.Empty;

    [MaxLength(500)]
    [Column("test_description")]
    public string? TestDescription { get; set; }

    [MaxLength(1000)]
    [Column("test_result")]
    public string? TestResult { get; set; }

    [Column("status")]
    public bool Status { get; set; } = false;

    public Patient? Patient { get; set; }
    public Practitioner? Practitioner { get; set; }
    public VisitRecord? VisitRecord { get; set; }
}

