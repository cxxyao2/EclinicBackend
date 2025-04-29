namespace EclinicBackend.Dtos;


public class AddPractitionerDto
{

    public string FirstName { get; set; } = string.Empty;


    public string LastName { get; set; } = string.Empty;


    public string? Specialty { get; set; }


    public string? PhoneNumber { get; set; }


    public string? Email { get; set; }

}

