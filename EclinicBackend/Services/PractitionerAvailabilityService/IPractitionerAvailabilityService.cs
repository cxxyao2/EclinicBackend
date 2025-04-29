using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.PractitionerAvailabilityService;

public interface IPractitionerAvailabilityService
{
    Task<ServiceResponse<List<GetPractitionerAvailabilityDto>>> GetAll();
    Task<ServiceResponse<GetPractitionerAvailabilityDto>> GetById(int id);
    Task<ServiceResponse<GetPractitionerAvailabilityDto>> Add(AddPractitionerAvailabilityDto newEntity);
    Task<ServiceResponse<GetPractitionerAvailabilityDto>> Update(GetPractitionerAvailabilityDto updatedEntity);
    Task<ServiceResponse<string>> Delete(int id);
}

