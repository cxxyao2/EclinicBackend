using System.Security.Claims;
using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.BedService;

public class BedService : IBedService
{
  private readonly IMapper _mapper;
  private readonly ApplicationDbContext _context;
  private readonly IHttpContextAccessor _httpContextAccessor;


  public BedService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
  {
    _mapper = mapper;
    _context = context;
    _httpContextAccessor = httpContextAccessor;
  }
  private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

  public async Task<ServiceResponse<List<GetBedDto>>> GetAll()
  {
    var serviceResponse = new ServiceResponse<List<GetBedDto>>();
    var bedDetails = await _context.Beds
     .Include(b => b.Inpatient)
       .ThenInclude(i => i!.Practitioner)
     .Include(b => b.Inpatient)
       .ThenInclude(i => i!.Patient)
      .OrderBy(b => b.BedId) // Sort by BedId in ascending order
     .Select(b => new GetBedDto
     {
       BedId = b.BedId,
       RoomNumber = b.RoomNumber,
       BedNumber = b.BedNumber,
       InpatientId = b.InpatientId,
       NurseId = b.Inpatient != null ? b.Inpatient.NurseId : null,
       NurseName = b.Inpatient != null && b.Inpatient.NurseId != null ? "NurseId: " + b.Inpatient.NurseId.ToString() : null,
       PractitionerId = b.Inpatient != null ? b.Inpatient.PractitionerId : 0,
       PractitionerName = b.Inpatient != null && b.Inpatient.Practitioner != null ? b.Inpatient.Practitioner.FirstName : null,
       PatientId = b.Inpatient != null ? b.Inpatient.PatientId : 0,
       PatientName = b.Inpatient != null && b.Inpatient.Patient != null ? b.Inpatient.Patient.FirstName : null
     })
     .ToListAsync();

    serviceResponse.Data = [.. bedDetails];
    serviceResponse.Message = $"Get {serviceResponse.Data.Count} Beds";
    return serviceResponse;
  }

  public async Task<ServiceResponse<UpdateBedDto>> Update(UpdateBedDto updatedEntity)
  {
    var serviceResponse = new ServiceResponse<UpdateBedDto>();
    try
    {
      var bed = await _context.Beds.FirstOrDefaultAsync(c => c.BedId == updatedEntity.BedId);
      if (bed == null)
        throw new Exception($"BedId {updatedEntity.BedId} does not exist");

      // Update PatientRecordId based on the incoming Dto
      bed.InpatientId = updatedEntity.InpatientId;
      bed.UpdatedAt = DateTime.UtcNow;
      bed.UpdatedBy = GetUserId();

      // Save changes to the database
      _context.Beds.Update(bed);
      await _context.SaveChangesAsync();

      // Return the updated bed data as Dto
      serviceResponse.Data = _mapper.Map<UpdateBedDto>(bed);
      serviceResponse.Success = true;
    }
    catch (Exception ex)
    {
      serviceResponse.Success = false;
      serviceResponse.Message = ex.Message;
    }

    return serviceResponse;

  }

  public async Task<ServiceResponse<List<UpdateBedDto>>> BatchUpdate(List<UpdateBedDto> updatedEntities)
  {
    var serviceResponse = new ServiceResponse<List<UpdateBedDto>>();
    try
    {
      var updatedBeds = new List<UpdateBedDto>();

      foreach (var updatedEntity in updatedEntities)
      {
        var bed = await _context.Beds.FirstOrDefaultAsync(c => c.BedId == updatedEntity.BedId);
        if (bed == null)
          continue; // Skip beds that don't exist

        // Update InpatientId based on the incoming Dto
        bed.InpatientId = updatedEntity.InpatientId;
        bed.UpdatedAt = DateTime.UtcNow;
        bed.UpdatedBy = GetUserId();

        updatedBeds.Add(_mapper.Map<UpdateBedDto>(bed));
      }

      // Save all changes to the database at once
      await _context.SaveChangesAsync();

      // Return the updated bed data as Dtos
      serviceResponse.Data = updatedBeds;
      serviceResponse.Success = true;
      serviceResponse.Message = $"Updated {updatedBeds.Count} beds successfully";
    }
    catch (Exception ex)
    {
      serviceResponse.Success = false;
      serviceResponse.Message = ex.Message;
    }

    return serviceResponse;
  }

}

