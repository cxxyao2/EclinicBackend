using System.ComponentModel.DataAnnotations;

namespace EclinicBackend.Dtos;

public class ActivateAccountDto
{
    [Required]
    [StringLength(6, MinimumLength = 6)]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Code must be exactly 6 digits")]
    public string Code { get; set; } = string.Empty;
}

public class ResendActivationCodeDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}