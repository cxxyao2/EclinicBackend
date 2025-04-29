using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.PrescriptionService;

public interface IPrescriptionService
{
    Task<ServiceResponse<List<GetPrescriptionDto>>> GetAll();
    Task<ServiceResponse<GetPrescriptionDto>> GetById(int id);
    Task<ServiceResponse<GetPrescriptionDto>> Add(AddPrescriptionDto newEntity);
    Task<ServiceResponse<GetPrescriptionDto>> Update(GetPrescriptionDto updatedEntity);
    Task<ServiceResponse<string>> Delete(int ID);
    Task<ServiceResponse<int>> BatchAdd(List<AddPrescriptionDto> prescriptions);
}

