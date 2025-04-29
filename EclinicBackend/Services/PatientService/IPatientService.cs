

using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.PatientService;

public interface IPatientService
{
  Task<ServiceResponse<List<GetPatientDto>>> GetAll();
  Task<ServiceResponse<GetPatientDto>> GetById(int id);
  Task<ServiceResponse<GetPatientDto>> Add(AddPatientDto newPatient);
  Task<ServiceResponse<GetPatientDto>> Update(GetPatientDto updatedPatient);
  Task<ServiceResponse<string>> Delete(int id);
}

