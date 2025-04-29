using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EclinicBackend.Enums;
using System.Text.Json.Serialization;

namespace EclinicBackend.Models;
public class Billing : AuditableEntity
{
    [Key]
    [Column("billing_id")]
    public int BillingID { get; set; }

    [Required]
    [Column("patient_id")]
    public int PatientID { get; set; }

    [Required]
    [Column("visit_id")]
    public int VisitID { get; set; }

    [Required]
    [Column("total_amount", TypeName = "decimal(18, 2)")]

    public decimal TotalAmount { get; set; }

    [Required]
    [Column("payment_status")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Pending;

    [Column("billing_date")]
    public DateTime? BillingDate { get; set; }


    public Patient? Patient { get; set; }



}


