namespace EclinicBackend.Dtos;
public class GetPractitionerDto
{
    public int PractitionerId { get; set; }


    public string FirstName { get; set; } = string.Empty;


    public string LastName { get; set; } = string.Empty;


    public string? Specialty { get; set; }


    public string? PhoneNumber { get; set; }


    public string? Email { get; set; }

}
