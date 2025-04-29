using EclinicBackend.Enums;
using System.Text.Json.Serialization;

namespace EclinicBackend.Dtos;
public class GetAppointmentDto
{

    public int AppointmentId { get; set; }


    public int PatientId { get; set; }
    public string? PatientName { get; set; }

    public int AvailableId { get; set; }


    public string? ReasonForVisit { get; set; }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AppointmentStatusEnum Status { get; set; } = AppointmentStatusEnum.Scheduled;  // Default to "Scheduled"
                                                                                          // status: check-in. will be shown on wait-list screen.





}


