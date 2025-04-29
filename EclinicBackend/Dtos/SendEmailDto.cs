namespace EclinicBackend.Dtos;
public class SendEmailDto
{
    public string ToEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
