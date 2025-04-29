namespace EclinicBackend.Dtos;
public class GetPractitionerAvailabilityDto
{
    public int AvailableId { get; set; }
    public int PractitionerId { get; set; }

    public string? PractitionerName { get; set; }

    public DateTime SlotDateTime { get; set; } // Stores date, time, and timezone offset
    public bool IsAvailable { get; set; }

}
