using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace EclinicBackend.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRole
    {
        [EnumMember(Value = "Nurse")]
        Nurse = 0,

        [EnumMember(Value = "Practitioner")]
        Practitioner = 1,

        [EnumMember(Value = "Patient")]
        Patient = 2,

        [EnumMember(Value = "Admin")]
        Admin = 3
    }
}
