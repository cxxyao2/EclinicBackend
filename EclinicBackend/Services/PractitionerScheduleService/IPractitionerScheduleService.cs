using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.PractitionerScheduleService;

public interface IPractitionerScheduleService
{
    Task<ServiceResponse<List<GetPractitionerScheduleDto>>> GetByPatient(int patientId);
    Task<ServiceResponse<List<GetPractitionerScheduleDto>>> GetByPractitionerAndDate(int practitionerId, DateTime workDate);
    Task<ServiceResponse<List<GetPractitionerScheduleDto>>> GetByDate(DateTime workDate);
    Task<ServiceResponse<List<GetPractitionerScheduleDto>>> GetAll();
    Task<ServiceResponse<GetPractitionerScheduleDto>> GetById(int id);
    Task<ServiceResponse<GetPractitionerScheduleDto>> Add(AddPractitionerScheduleDto newEntity);
    Task<ServiceResponse<GetPractitionerScheduleDto>> Update(UpdatePractitionerScheduleDto updatedEntity);
    Task<ServiceResponse<string>> Delete(int id);
    Task<ServiceResponse<string>> BatchDelete(List<int> ids);
    Task<ServiceResponse<List<GetPractitionerScheduleDto>>> AddBatch(List<AddPractitionerScheduleDto> newSchedules);
}

