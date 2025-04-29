using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.AppointmentService;

public interface IAppointmentService
{
    Task<ServiceResponse<List<GetAppointmentDto>>> GetAll();
    Task<ServiceResponse<GetAppointmentDto>> GetById(int id);
    Task<ServiceResponse<GetAppointmentDto>> Add(AddAppointmentDto newEntity);
    Task<ServiceResponse<GetAppointmentDto>> Update(GetAppointmentDto updatedEntity);
    Task<ServiceResponse<string>> Delete(int id);
}

