namespace EclinicBackend.Dtos;
public class AddPractitionerAvailabilityDto
{

    public int PractitionerId { get; set; }

    public DateTime SlotDateTime { get; set; } // Stores date, time, and timezone offset
    public bool IsAvailable { get; set; }

}
