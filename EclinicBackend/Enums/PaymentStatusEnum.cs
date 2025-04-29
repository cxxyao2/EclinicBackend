using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace EclinicBackend.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentStatusEnum
{
    [EnumMember(Value = "Pending")]
    Pending = 0,     // Default state when payment is still to be processed

    [EnumMember(Value = "Completed")]
    Completed = 1,   // Payment is successfully processed
    [EnumMember(Value = "Failed")]
    Failed = 2,      // Payment failed
    [EnumMember(Value = "Cancelled")]
    Cancelled = 3,   // Payment was cancelled
    [EnumMember(Value = "Refunded")]
    Refunded = 4   // Payment was refunded
}


