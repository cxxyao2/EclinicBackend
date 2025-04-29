using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EclinicBackend.Models;
public class Inpatient : AuditableEntity
{
    [Key]
    [Column("inpatient_id")]
    public int InpatientId { get; set; }

    [Required]
    [Column("patient_id")]
    public int PatientId { get; set; }

    [Required]
    [Column("practitioner_id")]
    public int PractitionerId { get; set; }

    [Column("nurse_id")]
    public int? NurseId { get; set; }

    [Required]
    [Column("admission_date")]
    public DateTime AdmissionDate { get; set; }

    [Column("discharge_date")]
    public DateTime? DischargeDate { get; set; }

    [MaxLength(20)]
    [Column("room_number")]
    public string? RoomNumber { get; set; }

    [MaxLength(10)]
    [Column("bed_number")]
    public string? BedNumber { get; set; }

    [MaxLength(500)]
    [Column("reason_for_admission")]
    public string? ReasonForAdmission { get; set; }




    public Patient? Patient { get; set; }


    public Practitioner? Practitioner { get; set; }


}

