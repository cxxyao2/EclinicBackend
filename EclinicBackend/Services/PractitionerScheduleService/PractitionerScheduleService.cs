using System.Security.Claims;
using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.PractitionerScheduleService;
public class PractitionerScheduleService : IPractitionerScheduleService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PractitionerScheduleService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<ServiceResponse<GetPractitionerScheduleDto>> Add(AddPractitionerScheduleDto newEntity)
    {
        var serviceResponse = new ServiceResponse<GetPractitionerScheduleDto>();
        var practitionerSchedule = _mapper.Map<PractitionerSchedule>(newEntity);
        practitionerSchedule.CreatedAt = DateTime.UtcNow;
        practitionerSchedule.CreatedBy = GetUserId();

        _context.PractitionerSchedules.Add(practitionerSchedule);
        await _context.SaveChangesAsync();

        serviceResponse.Data = _mapper.Map<GetPractitionerScheduleDto>(practitionerSchedule);

        return serviceResponse;
    }


    public async Task<ServiceResponse<List<GetPractitionerScheduleDto>>> GetAll()
    {
        var serviceResponse = new ServiceResponse<List<GetPractitionerScheduleDto>>();
        var dbPractitionerSchedules = await _context.PractitionerSchedules
        .Include(x => x.Patient)
        .Select(x => new GetPractitionerScheduleDto
        {
            ScheduleId = x.ScheduleId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ReasonForVisit = x.ReasonForVisit ?? "",
            StartDateTime = x.StartDateTime,
            EndDateTime = x.EndDateTime

        })
          .ToListAsync();
        serviceResponse.Data = dbPractitionerSchedules;
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetPractitionerScheduleDto>> GetById(int id)
    {
        var serviceResponse = new ServiceResponse<GetPractitionerScheduleDto>();
        var dbPractitionerSchedule = await _context.PractitionerSchedules
        .Include(x => x.Patient)
        .Select(x => new GetPractitionerScheduleDto
        {
            ScheduleId = x.ScheduleId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ReasonForVisit = x.ReasonForVisit ?? "",
            StartDateTime = x.StartDateTime,
            EndDateTime = x.EndDateTime
        })
          .FirstOrDefaultAsync(c => c.ScheduleId == id);
        serviceResponse.Data = _mapper.Map<GetPractitionerScheduleDto>(dbPractitionerSchedule);

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetPractitionerScheduleDto>> Update(UpdatePractitionerScheduleDto updatedEntity)
    {
        var serviceResponse = new ServiceResponse<GetPractitionerScheduleDto>();
        try
        {
            var practitionerSchedule = await _context.PractitionerSchedules
              .FirstOrDefaultAsync(c => c.ScheduleId == updatedEntity.ScheduleId);
            if (practitionerSchedule is null)
                throw new Exception($"PractitionerSchedule {updatedEntity.ScheduleId} not found");


            // Update only non-null fields
            practitionerSchedule.ReasonForVisit = updatedEntity.ReasonForVisit ?? practitionerSchedule.ReasonForVisit;
            practitionerSchedule.PatientId = updatedEntity.PatientId ?? practitionerSchedule.PatientId;
            practitionerSchedule.UpdatedBy = GetUserId();
            practitionerSchedule.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetPractitionerScheduleDto>(practitionerSchedule);
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

            var PractitionerSchedule = await _context.PractitionerSchedules.FirstOrDefaultAsync(c => c.ScheduleId == id) ?? throw new Exception($"PractitionerSchedule with Id {id} not found");
            _context.PractitionerSchedules.Remove(PractitionerSchedule);
            await _context.SaveChangesAsync();
            serviceResponse.Data = null;
            serviceResponse.Message = $"Deleted a PractitionerScheduleId {id}";

        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;

    }

    public async Task<ServiceResponse<List<GetPractitionerScheduleDto>>> GetByPractitionerAndDate(int practitionerId, DateTime workDate)
    {
        var serviceResponse = new ServiceResponse<List<GetPractitionerScheduleDto>>();
        var nextDay = workDate.AddDays(1);
        var dbPractitionerSchedules = await _context.PractitionerSchedules
        .Include(x => x.Patient)
        .Where(x => x.PractitionerId == practitionerId
           && x.StartDateTime >= workDate
         && x.StartDateTime < nextDay)
        .Select(x => new GetPractitionerScheduleDto
        {
            ScheduleId = x.ScheduleId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ReasonForVisit = x.ReasonForVisit ?? "",
            StartDateTime = x.StartDateTime,
            EndDateTime = x.EndDateTime
        })
          .ToListAsync();
        serviceResponse.Data = dbPractitionerSchedules;
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetPractitionerScheduleDto>>> GetByPatient(int patientId)
    {
        var serviceResponse = new ServiceResponse<List<GetPractitionerScheduleDto>>();
        var dbPractitionerSchedules = await _context.PractitionerSchedules
        .Include(x => x.Patient)
        .Where(x => x.PatientId == patientId)
        .Select(x => new GetPractitionerScheduleDto
        {
            ScheduleId = x.ScheduleId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ReasonForVisit = x.ReasonForVisit ?? "",
            StartDateTime = x.StartDateTime,
            EndDateTime = x.EndDateTime
        })
          .ToListAsync();
        serviceResponse.Data = dbPractitionerSchedules;
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetPractitionerScheduleDto>>> GetByDate(DateTime workDate)
    {
        var serviceResponse = new ServiceResponse<List<GetPractitionerScheduleDto>>();
        var nextDay = workDate.AddDays(1);
        var dbPractitionerSchedules = await _context.PractitionerSchedules
        .Include(x => x.Patient)
        .Where(x => x.StartDateTime >= workDate
         && x.StartDateTime < nextDay)
        .Select(x => new GetPractitionerScheduleDto
        {
            ScheduleId = x.ScheduleId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            ReasonForVisit = x.ReasonForVisit ?? "",
            StartDateTime = x.StartDateTime,
            EndDateTime = x.EndDateTime
        })
          .ToListAsync();
        serviceResponse.Data = dbPractitionerSchedules;
        return serviceResponse;
    }

    public async Task<ServiceResponse<string>> BatchDelete(List<int> ids)
    {
        try
        {
            var schedules = await _context.PractitionerSchedules
                .Where(s => ids.Contains(s.ScheduleId))
                .ToListAsync();

            if (schedules.Count == 0)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "No schedules found with the provided IDs"
                };
            }

            _context.PractitionerSchedules.RemoveRange(schedules);
            await _context.SaveChangesAsync();

            return new ServiceResponse<string>
            {
                Success = true,
                Data = $"Successfully deleted {schedules.Count} schedules",
                Message = "Schedules deleted successfully"
            };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<string>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ServiceResponse<List<GetPractitionerScheduleDto>>> AddBatch(List<AddPractitionerScheduleDto> newSchedules)
    {
        var serviceResponse = new ServiceResponse<List<GetPractitionerScheduleDto>>();
        var addedSchedules = new List<GetPractitionerScheduleDto>();

        try
        {
            var practitionerSchedules = newSchedules.Select(Dto =>
            {
                var schedule = _mapper.Map<PractitionerSchedule>(Dto);
                schedule.CreatedAt = DateTime.UtcNow;
                // schedule.CreatedBy = GetUserId();
                return schedule;
            }).ToList();

            _context.PractitionerSchedules.AddRange(practitionerSchedules);
            await _context.SaveChangesAsync();

            serviceResponse.Data = practitionerSchedules.Select(ps =>
                _mapper.Map<GetPractitionerScheduleDto>(ps)).ToList();
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }
}
