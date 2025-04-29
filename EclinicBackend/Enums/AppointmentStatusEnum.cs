using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace EclinicBackend.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AppointmentStatusEnum
{
    [EnumMember(Value = "Scheduled")]
    Scheduled = 0,   // The appointment is scheduled
    [EnumMember(Value = "CheckedIn")]
    CheckedIn = 1,
    [EnumMember(Value = "Completed")]

    Completed = 2,   // The appointment is completed
    [EnumMember(Value = "Cancelled")]
    Cancelled = 3,   // The appointment has been cancelled
}


