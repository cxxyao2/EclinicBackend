using EclinicBackend.Enums;

namespace EclinicBackend.Dtos;
public class AddInpatientDto
{

    public int PatientId { get; set; }

    public int PractitionerId { get; set; }

    public int? NurseId { get; set; }


    public DateTime AdmissionDate { get; set; }

    public DateTime? DischargeDate { get; set; }


    public string? RoomNumber { get; set; }


    public string? BedNumber { get; set; }


    public string? ReasonForAdmission { get; set; }



}

