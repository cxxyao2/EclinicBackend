namespace EclinicBackend.Dtos;

public class SignatureDto
{
    public required string Image { get; set; }
    public int VisitRecordId { get; set; }
    public int PatientId { get; set; }
}

