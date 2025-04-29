using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.MedicationService;

public interface IMedicationService
{
    Task<ServiceResponse<List<GetMedicationDto>>> GetAll();
    Task<ServiceResponse<GetMedicationDto>> GetById(int id);
    // Task<ServiceResponse<GetMedicationDto>> Add(AddMedicationDto newEntity);
    // Task<ServiceResponse<GetMedicationDto>> Update(GetMedicationDto updatedEntity);
    // Task<ServiceResponse<string>> Delete(int id);
}

