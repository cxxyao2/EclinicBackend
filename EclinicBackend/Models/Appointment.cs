using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EclinicBackend.Enums;

namespace EclinicBackend.Models;
// depracated. 2024. Dec.
public class Appointment
{
    [Key]
    [Column("appointment_id")]
    public int AppointmentId { get; set; }

    [Required]
    [Column("patient_id")]
    public int PatientId { get; set; }
    [Required]
    [Column("available_id")]
    public int AvailableId { get; set; }


    [MaxLength(255)]
    [Column("reason_for_visit")]
    public string? ReasonForVisit { get; set; }

    [Required]
    [Column("status")]
    public AppointmentStatusEnum Status { get; set; } = AppointmentStatusEnum.Scheduled;  // Default to "Scheduled"
    // status: check-in. will be shown on wait-list screen.


    public Patient? Patient { get; set; }

    public PractitionerAvailability? PractitionerAvailability { get; set; }
}


