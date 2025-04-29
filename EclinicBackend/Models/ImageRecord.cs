using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EclinicBackend.Models;
public class ImageRecord : AuditableEntity
{
    [Key]
    [Column("image_id")]
    public int ImageId { get; set; }

    [Required]
    [Column("patient_id")]
    public int PatientId { get; set; }

    [Column("practitioner_id")]
    public int? PractitionerId { get; set; }

    [Column("inpatient_id")]
    public int? InpatientId { get; set; }

    [MaxLength(50)]
    [Column("image_type")]
    public string? ImageType { get; set; }

    [MaxLength(500)]
    [Column("image_description")]
    public string? ImageDescription { get; set; }

    [Required]
    [MaxLength(500)]
    [Column("image_path")]
    public required string ImagePath { get; set; }

    [Required]
    [Column("capture_date")]
    public DateTime CaptureDate { get; set; }

    [MaxLength(20)]
    [Column("status")]
    public string? Status { get; set; }

    public Patient? Patient { get; set; }
    public Practitioner? Practitioner { get; set; }
    public Inpatient? Inpatient { get; set; }
}


