namespace EclinicBackend.Dtos;
public class AddPrescriptionDto
{

    public int VisitId { get; set; }
    public int PatientId { get; set; }


    public int PractitionerId { get; set; }



    public int MedicationId { get; set; }

    public string? Dosage { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }


    public string? Notes { get; set; }
}