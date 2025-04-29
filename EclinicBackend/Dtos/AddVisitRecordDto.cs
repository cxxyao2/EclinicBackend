using EclinicBackend.Enums;

namespace EclinicBackend.Dtos;

public class AddVisitRecordDto
{


    public int PatientId { get; set; }

    public int PractitionerId { get; set; }

    public int ScheduleId { get; set; }

    public string PractitionerSignaturePath { get; set; } = "";


    public DateTime VisitDate { get; set; }


    public string Diagnosis { get; set; } = "";


    public string Treatment { get; set; } = "";


    public string Notes { get; set; } = "";


}

