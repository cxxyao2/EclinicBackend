using EclinicBackend.Enums;
using System.Text.Json.Serialization;

namespace EclinicBackend.Dtos;
public class AddAppointmentDto
{
    public int PatientId { get; set; }

    public int AvailableId { get; set; }


    public string? ReasonForVisit { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AppointmentStatusEnum Status { get; set; } = AppointmentStatusEnum.Scheduled;  // Default to "Scheduled"



}


