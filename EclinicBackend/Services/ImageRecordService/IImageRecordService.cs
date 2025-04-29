using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.ImageRecordService;

public interface IImageRecordService
{
    Task<ServiceResponse<List<GetImageRecordDto>>> GetAll();
    Task<ServiceResponse<GetImageRecordDto>> GetById(int id);
    // Task<ServiceResponse<GetImageRecordDto>> Add(AddImageRecordDto newEntity);
    // Task<ServiceResponse<GetImageRecordDto>> Update(GetImageRecordDto updatedEntity);
    // Task<ServiceResponse<string>> Delete(int id);
}

