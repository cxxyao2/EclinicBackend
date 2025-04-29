namespace EclinicBackend.Dtos;

using System.ComponentModel.DataAnnotations;

public class RequestPasswordResetDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare("NewPassword", ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}