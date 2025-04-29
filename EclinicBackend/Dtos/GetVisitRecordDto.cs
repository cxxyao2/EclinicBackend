using EclinicBackend.Enums;

namespace EclinicBackend.Dtos;

public class GetVisitRecordDto
{
    public GetVisitRecordDto()
    {
        Diagnosis = string.Empty;
        Treatment = string.Empty;
        Notes = string.Empty;
    }

    public int VisitId { get; set; }
    public int PatientId { get; set; }

    public string? PatientName { get; set; }

    public int PractitionerId { get; set; }
    public string? PractitionerName { get; set; }

    public int ScheduleId { get; set; }

    public string PractitionerSignaturePath { get; set; } = "";


    public DateTime VisitDate { get; set; }


    public string Diagnosis { get; set; }


    public string Treatment { get; set; }


    public string Notes { get; set; }
}

