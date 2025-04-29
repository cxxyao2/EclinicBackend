using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.BedService;

public interface IBedService
{

  Task<ServiceResponse<List<GetBedDto>>> GetAll();
  Task<ServiceResponse<UpdateBedDto>> Update(UpdateBedDto updatedEntity);
  Task<ServiceResponse<List<UpdateBedDto>>> BatchUpdate(List<UpdateBedDto> updatedEntities);

}

