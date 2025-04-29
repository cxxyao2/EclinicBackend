namespace EclinicBackend.Dtos;
public class PrescriptionDto
{
    public int PrescriptionId { get; set; }

    public int PatientId { get; set; }
    public string? PatientFirstName { get; set; }
    public string? PatientLastName { get; set; }
    public string? PractitionerFirstName { get; set; }
    public string? PractitionerLastName { get; set; }

    public int PractitionerId { get; set; }


    public int MedicationId { get; set; }
    public string? MedicationName { get; set; }

    public string? Dosage { get; set; }


}
