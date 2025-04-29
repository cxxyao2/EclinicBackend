using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.LabTestService;

public interface ILabTestService
{
    Task<ServiceResponse<List<GetLabTestDto>>> GetAll();
    Task<ServiceResponse<GetLabTestDto>> GetById(int id);
    // Task<ServiceResponse<GetLabTestDto>> Add(AddLabTestDto newEntity);
    // Task<ServiceResponse<GetLabTestDto>> Update(GetLabTestDto updatedEntity);
    // Task<ServiceResponse<string>> Delete(int id);
}

