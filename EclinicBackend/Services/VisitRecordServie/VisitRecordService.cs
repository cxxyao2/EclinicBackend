using System.Security.Claims;
using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EclinicBackend.Services.VisitRecordService;
public class VisitRecordService : IVisitRecordService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<VisitRecordService> _logger;

    public VisitRecordService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<VisitRecordService> logger)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<ServiceResponse<GetVisitRecordDto>> Add(AddVisitRecordDto newVisitRecord)
    {

        var serviceResponse = new ServiceResponse<GetVisitRecordDto>();
        try
        {
            var visitRecord = _mapper.Map<VisitRecord>(newVisitRecord);
            visitRecord.CreatedBy = GetUserId();
            visitRecord.CreatedAt = DateTime.UtcNow;

            _context.VisitRecords.Add(visitRecord);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetVisitRecordDto>(visitRecord);

            return serviceResponse;

        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
            return serviceResponse;
        }

    }


    public async Task<ServiceResponse<List<GetVisitRecordDto>>> GetAll()
    {
        var serviceResponse = new ServiceResponse<List<GetVisitRecordDto>>();
        var dbVisitRecords = await _context.VisitRecords
         .Include(x => x.Patient)
        .Include(x => x.Practitioner)
        .Select(x => new GetVisitRecordDto
        {
            VisitId = x.VisitId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ScheduleId = x.ScheduleId,
            PractitionerSignaturePath = x.PractitionerSignaturePath,
            VisitDate = x.VisitDate,
            Diagnosis = x.Diagnosis,
            Treatment = x.Treatment,
            Notes = x.Notes
        })
          .ToListAsync();
        serviceResponse.Data = dbVisitRecords;
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetVisitRecordDto>> GetById(int id)
    {
        var serviceResponse = new ServiceResponse<GetVisitRecordDto>();
        var dbVisitRecord = await _context.VisitRecords
        .Include(x => x.Patient)
        .Include(x => x.Practitioner)
        .Select(x => new GetVisitRecordDto
        {
            VisitId = x.VisitId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ScheduleId = x.ScheduleId,
            PractitionerSignaturePath = x.PractitionerSignaturePath,
            VisitDate = x.VisitDate,
            Diagnosis = x.Diagnosis,
            Treatment = x.Treatment,
            Notes = x.Notes
        })
          .FirstOrDefaultAsync(c => c.VisitId == id);
        serviceResponse.Data = dbVisitRecord;

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetVisitRecordDto>> Update(GetVisitRecordDto updatedVisitRecord)
    {
        var serviceResponse = new ServiceResponse<GetVisitRecordDto>();
        try
        {
            var visitRecord = await _context.VisitRecords
              .FirstOrDefaultAsync(c => c.VisitId == updatedVisitRecord.VisitId);
            if (visitRecord is null)
                throw new Exception($"VisitRecord {updatedVisitRecord.VisitId} not found");


            // Update only Diagnosis, Treatment, Notes
            visitRecord.Diagnosis = updatedVisitRecord.Diagnosis;
            visitRecord.Treatment = updatedVisitRecord.Treatment;
            visitRecord.Notes = updatedVisitRecord.Notes;
            visitRecord.UpdatedAt = DateTime.UtcNow;
            visitRecord.UpdatedBy = GetUserId();

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetVisitRecordDto>(visitRecord);
        }
        catch (Exception ex)
        {
            serviceResponse.Data = null;
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;

    }



    public async Task<ServiceResponse<string>> Delete(int id)
    {
        var serviceResponse = new ServiceResponse<string>();

        try
        {

            var VisitRecord = await _context.VisitRecords.FirstOrDefaultAsync(c => c.VisitId == id) ?? throw new Exception($"VisitRecord with Id {id} not found");
            _context.VisitRecords.Remove(VisitRecord);
            await _context.SaveChangesAsync();
            serviceResponse.Data = null;
            serviceResponse.Message = $"Deleted a VisitRecordId {id}";

        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;

    }

    public async Task<ServiceResponse<List<GetVisitRecordDto>>> GetByPatient(int patientId)
    {
        var serviceResponse = new ServiceResponse<List<GetVisitRecordDto>>();
        var dbVisitRecords = await _context.VisitRecords
         .Include(x => x.Patient)
        .Include(x => x.Practitioner)
        .Where(x => x.PatientId == patientId)
        .Select(x => new GetVisitRecordDto
        {
            VisitId = x.VisitId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ScheduleId = x.ScheduleId,
            PractitionerSignaturePath = x.PractitionerSignaturePath,
            VisitDate = x.VisitDate,
            Diagnosis = x.Diagnosis,
            Treatment = x.Treatment,
            Notes = x.Notes
        })
          .ToListAsync();
        serviceResponse.Data = dbVisitRecords;
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetVisitRecordDto>>> GetByPractitionerAndDate(int practitionerId, DateTime visitDate)
    {
        var serviceResponse = new ServiceResponse<List<GetVisitRecordDto>>();
        var dbVisitRecords = await _context.VisitRecords
         .Include(x => x.Patient)
        .Include(x => x.Practitioner)
        .Where(x => x.PractitionerId == practitionerId
            && x.VisitDate >= visitDate)
        .Select(x => new GetVisitRecordDto
        {
            VisitId = x.VisitId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ScheduleId = x.ScheduleId,
            PractitionerSignaturePath = x.PractitionerSignaturePath,
            VisitDate = x.VisitDate,
            Diagnosis = x.Diagnosis,
            Treatment = x.Treatment,
            Notes = x.Notes
        })
          .ToListAsync();
        serviceResponse.Data = dbVisitRecords;
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetVisitRecordDto>>> GetByDate(DateTime visitDate)
    {
        var serviceResponse = new ServiceResponse<List<GetVisitRecordDto>>();
        var dbVisitRecords = await _context.VisitRecords
         .Include(x => x.Patient)
        .Include(x => x.Practitioner)
        .Where(x => x.VisitDate.Date == visitDate.Date)
        .Select(x => new GetVisitRecordDto
        {
            VisitId = x.VisitId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ScheduleId = x.ScheduleId,
            PractitionerSignaturePath = x.PractitionerSignaturePath,
            VisitDate = x.VisitDate,
            Diagnosis = x.Diagnosis,
            Treatment = x.Treatment,
            Notes = x.Notes
        })
          .ToListAsync();
        serviceResponse.Data = dbVisitRecords;
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetVisitRecordDto>>> GetWaitingList(DateTime visitDate)
    {
        var serviceResponse = new ServiceResponse<List<GetVisitRecordDto>>();
        var dbVisitRecords = await _context.VisitRecords
         .Include(x => x.Patient)
        .Include(x => x.Practitioner)
         .Where(x => x.VisitDate >= visitDate
         && x.Notes.Equals("")
         )
        .Select(x => new GetVisitRecordDto
        {
            VisitId = x.VisitId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ScheduleId = x.ScheduleId,
            PractitionerSignaturePath = x.PractitionerSignaturePath,
            VisitDate = x.VisitDate,
            Diagnosis = x.Diagnosis,
            Treatment = x.Treatment,
            Notes = x.Notes
        })
          .ToListAsync();
        serviceResponse.Data = dbVisitRecords;
        return serviceResponse;
    }


    public async Task<ServiceResponse<PaginatedResponse<GetMedicalHistoryDto>>> SearchSimilarCases(
        string searchTerm,
        DateTime? startDate = null,
        int page = 1,
        int pageSize = 10)
    {
        var serviceResponse = new ServiceResponse<PaginatedResponse<GetMedicalHistoryDto>>();
        var response = new PaginatedResponse<GetMedicalHistoryDto>();

        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Search term cannot be empty";
                return serviceResponse;
            }

            // Convert search term to lowercase for case-insensitive search
            searchTerm = searchTerm.ToLower().Trim();

            var query = _context.VisitRecords
                .Include(x => x.Practitioner)
                .Include(x => x.Patient)
                .Where(x => !string.IsNullOrEmpty(x.Diagnosis))
                .Where(x => x.Diagnosis.ToLower().Contains(searchTerm) ||
                           x.Treatment.ToLower().Contains(searchTerm));

            // Add date filter if provided
            if (startDate.HasValue)
            {
                query = query.Where(x => x.VisitDate.Date >= startDate.Value.Date);
            }

            // Get total count before pagination
            response.TotalItems = await query.CountAsync();
            response.PageSize = pageSize;
            response.CurrentPage = page;
            response.TotalPages = (int)Math.Ceiling(response.TotalItems / (double)pageSize);

            var similarCases = await query
                .OrderByDescending(x => x.VisitDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new GetMedicalHistoryDto
                {
                    VisitId = x.VisitId,
                    PatientId = x.PatientId,
                    PatientName = (x.Patient != null)
                        ? $"{x.Patient.FirstName} {x.Patient.LastName}"
                        : string.Empty,
                    PractitionerId = x.PractitionerId,
                    PractitionerName = (x.Practitioner != null)
                        ? $"{x.Practitioner.FirstName} {x.Practitioner.LastName}"
                        : string.Empty,
                    VisitDate = x.VisitDate,
                    Diagnosis = x.Diagnosis,
                    Treatment = x.Treatment
                })
                .ToListAsync();

            response.Items = similarCases;
            serviceResponse.Data = response;

            if (similarCases.Count == 0)
            {
                serviceResponse.Message = "No similar cases found.";
            }
            else
            {
                serviceResponse.Message = $"Found {response.TotalItems} similar cases. Showing page {page} of {response.TotalPages}.";
            }
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

}