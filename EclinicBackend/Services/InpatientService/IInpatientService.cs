using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.InpatientService;

public interface IInpatientService
{
    Task<List<GetInpatientDto>> GetPatientsWithoutNurse();
    Task<ServiceResponse<List<GetInpatientDto>>> GetInpatientsByRoomNumber(string roomNumber);
    Task<ServiceResponse<List<GetInpatientDto>>> GetAll();
    Task<ServiceResponse<GetInpatientDto>> GetById(int id);
    Task<ServiceResponse<GetInpatientDto>> Add(AddInpatientDto newEntity);
    Task<ServiceResponse<GetInpatientDto>> Update(GetInpatientDto updatedEntity);
    Task<ServiceResponse<string>> Delete(int id);
    Task<ServiceResponse<List<GetInpatientDto>>> BatchUpdate(List<GetInpatientDto> updatedInpatients);
}

