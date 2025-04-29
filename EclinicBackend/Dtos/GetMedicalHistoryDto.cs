using System.ComponentModel.DataAnnotations;

namespace EclinicBackend.Dtos;

public class GetMedicalHistoryDto
{
    public int VisitId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int PractitionerId { get; set; }
    public string PractitionerName { get; set; } = string.Empty;

    [Required]
    public DateTime VisitDate { get; set; } = DateTime.UtcNow; // Make sure this is not nullable
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
}

