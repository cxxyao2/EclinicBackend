using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.PractitionerService;

public interface IPractitionerService
{
  Task<ServiceResponse<List<GetPractitionerDto>>> GetAll();
  Task<ServiceResponse<GetPractitionerDto>> GetById(int id);
  Task<ServiceResponse<GetPractitionerDto>> Add(AddPractitionerDto newPractitioner);
  Task<ServiceResponse<GetPractitionerDto>> Update(GetPractitionerDto updatedPractitioner);
  Task<ServiceResponse<string>> Delete(int id);
}

