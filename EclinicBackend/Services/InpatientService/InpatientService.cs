using System.Security.Claims;
using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.InpatientService;


public class InpatientService : IInpatientService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public InpatientService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<ServiceResponse<GetInpatientDto>> Add(AddInpatientDto newInpatient)
    {
        var serviceResponse = new ServiceResponse<GetInpatientDto>();
        try
        {
            var inpatient = _mapper.Map<Inpatient>(newInpatient);
            inpatient.CreatedAt = DateTime.UtcNow;
            inpatient.CreatedBy = GetUserId();

            _context.Inpatients.Add(inpatient);
            await _context.SaveChangesAsync();

            serviceResponse.Data = null;
            serviceResponse.Success = true;
            serviceResponse.Message = "Inpatient record created successfully";

            return serviceResponse;
        }
        catch (Exception ex)
        {
            serviceResponse.Data = null;
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
            return serviceResponse;
        }


    }

    public async Task<List<GetInpatientDto>> GetPatientsWithoutNurse()
    {
        var dbInpatients = await _context.Inpatients
        .Include(x => x.Patient)
        .Include(x => x.Practitioner)
        .Where(x => (x.NurseId == null || x.NurseId == 0))
        .Select(x => new GetInpatientDto
        {
            InpatientId = x.InpatientId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            NurseId = x.NurseId,
            NurseName = "NurseId: " + x.NurseId,
            AdmissionDate = x.AdmissionDate,
            DischargeDate = x.DischargeDate,
            RoomNumber = x.RoomNumber,
            BedNumber = x.BedNumber,
            ReasonForAdmission = x.ReasonForAdmission,
        })
          .ToListAsync();
        return dbInpatients; ;
    }




    public async Task<ServiceResponse<List<GetInpatientDto>>> GetInpatientsByRoomNumber(string roomNumber)
    {
        var serviceResponse = new ServiceResponse<List<GetInpatientDto>>();

        var dbInpatients = await _context.Inpatients
        .Where(x => x.RoomNumber == roomNumber)
        .Include(x => x.Patient)
        .Include(x => x.Practitioner)
        .Select(x => new GetInpatientDto
        {
            InpatientId = x.InpatientId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            NurseId = x.NurseId,
            NurseName = "NurseId: " + x.NurseId,
            AdmissionDate = x.AdmissionDate,
            DischargeDate = x.DischargeDate,
            RoomNumber = x.RoomNumber,
            BedNumber = x.BedNumber,
            ReasonForAdmission = x.ReasonForAdmission,
        })
          .ToListAsync();
        serviceResponse.Data = dbInpatients;
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetInpatientDto>>> GetAll()
    {
        var serviceResponse = new ServiceResponse<List<GetInpatientDto>>();
        var dbInpatients = await _context.Inpatients
        .Include(x => x.Patient)
        .Include(x => x.Practitioner)
        .Select(x => new GetInpatientDto
        {
            InpatientId = x.InpatientId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            NurseId = x.NurseId,
            NurseName = "NurseId: " + x.NurseId,
            AdmissionDate = x.AdmissionDate,
            DischargeDate = x.DischargeDate,
            RoomNumber = x.RoomNumber,
            BedNumber = x.BedNumber,
            ReasonForAdmission = x.ReasonForAdmission,
        })
          .ToListAsync();
        serviceResponse.Data = dbInpatients;
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetInpatientDto>> GetById(int id)
    {
        var serviceResponse = new ServiceResponse<GetInpatientDto>();
        var dbInpatient = await _context.Inpatients
          .FirstOrDefaultAsync(c => c.InpatientId == id);
        serviceResponse.Data = _mapper.Map<GetInpatientDto>(dbInpatient);

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetInpatientDto>> Update(GetInpatientDto updatedInpatient)
    {
        var serviceResponse = new ServiceResponse<GetInpatientDto>();
        try
        {
            var inpatient = await _context.Inpatients
              .FirstOrDefaultAsync(c => c.InpatientId == updatedInpatient.InpatientId);
            if (inpatient is null)
                throw new Exception($"Inpatient {updatedInpatient.InpatientId} not found");


            // Update only NurserId, DischargeDate, RoomNumber, BedNumber
            if (updatedInpatient.NurseId != null)
                inpatient.NurseId = updatedInpatient.NurseId;

            if (updatedInpatient.DischargeDate != null)
                inpatient.DischargeDate = updatedInpatient.DischargeDate;



            inpatient.UpdatedAt = DateTime.UtcNow;
            inpatient.UpdatedBy = GetUserId();



            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetInpatientDto>(inpatient);
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

            var inpatient = await _context.Inpatients.FirstOrDefaultAsync(c => c.InpatientId == id) ?? throw new Exception($"Inpatient with Id {id} not found");
            _context.Inpatients.Remove(inpatient);
            await _context.SaveChangesAsync();
            serviceResponse.Data = null;
            serviceResponse.Message = $"Deleted a InpatientId {id}";

        }
        catch (Exception ex)
        {
            serviceResponse.Data = null;
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;

    }

    public async Task<ServiceResponse<List<GetInpatientDto>>> BatchUpdate(List<GetInpatientDto> updatedInpatients)
    {
        var serviceResponse = new ServiceResponse<List<GetInpatientDto>>();
        try
        {
            var updatedList = new List<GetInpatientDto>();

            foreach (var updatedInpatient in updatedInpatients)
            {
                var inpatient = await _context.Inpatients
                    .FirstOrDefaultAsync(i => i.InpatientId == updatedInpatient.InpatientId);

                if (inpatient == null)
                    continue; // Skip inpatients that don't exist

                // Update fields
                inpatient.NurseId = updatedInpatient.NurseId;
                inpatient.RoomNumber = updatedInpatient.RoomNumber;
                inpatient.BedNumber = updatedInpatient.BedNumber;
                inpatient.UpdatedAt = DateTime.UtcNow;
                inpatient.UpdatedBy = GetUserId();

                updatedList.Add(_mapper.Map<GetInpatientDto>(inpatient));
            }

            // Save all changes at once
            await _context.SaveChangesAsync();

            serviceResponse.Data = updatedList;
            serviceResponse.Success = true;
            serviceResponse.Message = $"Updated {updatedList.Count} inpatients successfully";
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;
    }

}
