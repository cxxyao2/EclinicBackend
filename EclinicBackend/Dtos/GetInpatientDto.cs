using EclinicBackend.Enums;

namespace EclinicBackend.Dtos;
public class GetInpatientDto
{

    public int InpatientId { get; set; }


    public int PatientId { get; set; }
    public string? PatientName { get; set; }

    public int PractitionerId { get; set; }
    public string? PractitionerName { get; set; }

    public int? NurseId { get; set; }
    public string? NurseName { get; set; }


    public DateTime AdmissionDate { get; set; }

    public DateTime? DischargeDate { get; set; }


    public string? RoomNumber { get; set; }


    public string? BedNumber { get; set; }


    public string? ReasonForAdmission { get; set; }







}

