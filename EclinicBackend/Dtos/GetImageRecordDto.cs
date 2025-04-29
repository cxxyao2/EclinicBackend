namespace EclinicBackend.Dtos;

public class GetImageRecordDto
{

    public int ImageId { get; set; }


    public int PatientId { get; set; }
    public string? PatientName { get; set; }

    public int? PractitionerId { get; set; }
    public string? PractitionerName { get; set; }

    public int? InpatientId { get; set; }

    public string? ImageType { get; set; }


    public string? ImageDescription { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public DateTime CaptureDate { get; set; }


    public string? Status { get; set; }





}
