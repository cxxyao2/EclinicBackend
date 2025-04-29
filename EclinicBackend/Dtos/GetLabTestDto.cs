namespace EclinicBackend.Dtos;

public class GetLabTestDto
{
    public int LabTestId { get; set; }

    public int PatientId { get; set; }
    public string? PatientName { get; set; }

    public int PractitionerId { get; set; }
    public string? PractitionerName { get; set; }

    public string? TestName { get; set; }

    public string? TestResult { get; set; }

    public DateTime? TestDate { get; set; }




}


