using System.Security.Claims;
using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.PractitionerService;
public class PractitionerService : IPractitionerService
{
  private readonly IMapper _mapper;
  private readonly ApplicationDbContext _context;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public static readonly List<string> ValidSpecialties = new List<string>
{
    "Internal Medicine",
    "Pediatrics",
    "Surgery",
    "Obstetrics and Gynecology",
    "Psychiatry",
    "Dermatology",
    "Radiology",
    "Cardiology",
    "Anesthesiology",
    "Emergency Medicine"
};


  public PractitionerService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
  {
    _mapper = mapper;
    _context = context;
    _httpContextAccessor = httpContextAccessor;
  }

  private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

  public async Task<ServiceResponse<GetPractitionerDto>> Add(AddPractitionerDto newPractitioner)
  {
    var serviceResponse = new ServiceResponse<GetPractitionerDto>();

    // Check if the specialty is valid
    if (!ValidSpecialties.Contains(newPractitioner.Specialty, StringComparer.OrdinalIgnoreCase))
    {
      serviceResponse.Data = null;
      serviceResponse.Success = false;
      serviceResponse.Message = "Please choose the right specialty.";
      return serviceResponse;
    }

    var practitioner = _mapper.Map<Practitioner>(newPractitioner);
    // practitioner.CreatedBy = GetUserId();
    practitioner.CreatedAt = DateTime.UtcNow;

    _context.Practitioners.Add(practitioner);
    await _context.SaveChangesAsync();

    serviceResponse.Data = _mapper.Map<GetPractitionerDto>(practitioner);
    serviceResponse.Success = true;
    serviceResponse.Message = $"Practitioner {practitioner.PractitionerId} created.";
    return serviceResponse;
  }


  public async Task<ServiceResponse<List<GetPractitionerDto>>> GetAll()
  {
    var serviceResponse = new ServiceResponse<List<GetPractitionerDto>>();
    var dbPractitioners = await _context.Practitioners
      .ToListAsync();
    serviceResponse.Data = dbPractitioners.Select(c => _mapper.Map<GetPractitionerDto>(c)).ToList();
    return serviceResponse;
  }

  public async Task<ServiceResponse<GetPractitionerDto>> GetById(int id)
  {
    var serviceResponse = new ServiceResponse<GetPractitionerDto>();
    var dbPractitioner = await _context.Practitioners
      .FirstOrDefaultAsync(c => c.PractitionerId == id);
    serviceResponse.Data = _mapper.Map<GetPractitionerDto>(dbPractitioner);
    serviceResponse.Success = true;
    return serviceResponse;
  }

  public async Task<ServiceResponse<GetPractitionerDto>> Update(GetPractitionerDto updatedPractitioner)
  {
    var serviceResponse = new ServiceResponse<GetPractitionerDto>();
    try
    {
      var practitioner = await _context.Practitioners
        .FirstOrDefaultAsync(c => c.PractitionerId == updatedPractitioner.PractitionerId);
      if (practitioner is null)
        throw new Exception($"Practitioner {updatedPractitioner.PractitionerId} not found");

      // Check if the specialty is valid
      if (!ValidSpecialties.Contains(updatedPractitioner.Specialty, StringComparer.OrdinalIgnoreCase))
      {
        serviceResponse.Success = false;
        serviceResponse.Message = "Please choose the right specialty.";
        return serviceResponse;
      }

      // Update only non-null fields
      if (!string.IsNullOrEmpty(updatedPractitioner.FirstName))
        practitioner.FirstName = updatedPractitioner.FirstName;

      if (!string.IsNullOrEmpty(updatedPractitioner.LastName))
        practitioner.LastName = updatedPractitioner.LastName;


      if (!string.IsNullOrEmpty(updatedPractitioner.Specialty))
        practitioner.Specialty = updatedPractitioner.Specialty;

      if (!string.IsNullOrEmpty(updatedPractitioner.PhoneNumber))
        practitioner.PhoneNumber = updatedPractitioner.PhoneNumber;

      if (!string.IsNullOrEmpty(updatedPractitioner.Email))
        practitioner.Email = updatedPractitioner.Email;

      practitioner.UpdatedAt = DateTime.UtcNow;
      // practitioner.UpdatedBy = GetUserId();

      await _context.SaveChangesAsync();
      serviceResponse.Data = _mapper.Map<GetPractitionerDto>(practitioner);
      serviceResponse.Success = true;
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

      var Practitioner = await _context.Practitioners.FirstOrDefaultAsync(c => c.PractitionerId == id) ?? throw new Exception($"Practitioner with Id {id} not found");
      _context.Practitioners.Remove(Practitioner);
      await _context.SaveChangesAsync();
      serviceResponse.Data = null;
      serviceResponse.Success = true;
      serviceResponse.Message = $"Deleted a PractitionerId {id}";

    }
    catch (Exception ex)
    {
      serviceResponse.Success = false;
      serviceResponse.Message = ex.Message;
    }

    return serviceResponse;

  }

}
