using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.VisitRecordService;

public interface IVisitRecordService
{
    Task<ServiceResponse<List<GetVisitRecordDto>>> GetWaitingList(DateTime visitDate);
    Task<ServiceResponse<List<GetVisitRecordDto>>> GetByPatient(int patientId);
    Task<ServiceResponse<List<GetVisitRecordDto>>> GetByPractitionerAndDate(int practitionerId, DateTime visitDate);
    Task<ServiceResponse<List<GetVisitRecordDto>>> GetByDate(DateTime visitDate);
    Task<ServiceResponse<List<GetVisitRecordDto>>> GetAll();
    Task<ServiceResponse<GetVisitRecordDto>> GetById(int id);
    Task<ServiceResponse<GetVisitRecordDto>> Add(AddVisitRecordDto newEntity);
    Task<ServiceResponse<GetVisitRecordDto>> Update(GetVisitRecordDto updatedEntity);
    Task<ServiceResponse<string>> Delete(int id);
    Task<ServiceResponse<PaginatedResponse<GetMedicalHistoryDto>>> SearchSimilarCases(
       string searchTerm,
       DateTime? startDate = null,
       int page = 1,
       int pageSize = 10);

}

