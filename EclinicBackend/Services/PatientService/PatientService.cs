using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.PatientService;
public class PatientService : IPatientService
{
  private readonly IMapper _mapper;
  private readonly ApplicationDbContext _context;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public PatientService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
  {
    _mapper = mapper;
    _context = context;
    _httpContextAccessor = httpContextAccessor;
  }

  // private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

  public async Task<ServiceResponse<GetPatientDto>> Add(AddPatientDto newPatient)
  {
    var serviceResponse = new ServiceResponse<GetPatientDto>();
    var patient = _mapper.Map<Patient>(newPatient);

    _context.Patients.Add(patient);
    await _context.SaveChangesAsync();

    serviceResponse.Data = _mapper.Map<GetPatientDto>(patient);

    return serviceResponse;
  }


  public async Task<ServiceResponse<List<GetPatientDto>>> GetAll()
  {
    var serviceResponse = new ServiceResponse<List<GetPatientDto>>();
    var dbPatients = await _context.Patients
      .ToListAsync();
    serviceResponse.Data = dbPatients.Select(c => _mapper.Map<GetPatientDto>(c)).ToList();
    return serviceResponse;
  }

  public async Task<ServiceResponse<GetPatientDto>> GetById(int id)
  {
    var serviceResponse = new ServiceResponse<GetPatientDto>();
    var dbPatient = await _context.Patients
      .FirstOrDefaultAsync(c => c.PatientId == id);
    serviceResponse.Data = _mapper.Map<GetPatientDto>(dbPatient);

    return serviceResponse;
  }

  public async Task<ServiceResponse<GetPatientDto>> Update(GetPatientDto updatedPatient)
  {
    var serviceResponse = new ServiceResponse<GetPatientDto>();
    try
    {
      var patient = await _context.Patients
        .FirstOrDefaultAsync(c => c.PatientId == updatedPatient.PatientId);
      if (patient is null)
        throw new Exception($"Patient {updatedPatient.PatientId} not found");


      // Update only non-null fields
      if (!string.IsNullOrEmpty(updatedPatient.FirstName))
        patient.FirstName = updatedPatient.FirstName;

      if (!string.IsNullOrEmpty(updatedPatient.LastName))
        patient.LastName = updatedPatient.LastName;

      // DateOfBirth is non-nullable in UpdatePatientDto, so it will always update
      patient.DateOfBirth = updatedPatient.DateOfBirth;

      if (!string.IsNullOrEmpty(updatedPatient.Gender))
        patient.Gender = updatedPatient.Gender;

      if (!string.IsNullOrEmpty(updatedPatient.Address))
        patient.Address = updatedPatient.Address;

      if (!string.IsNullOrEmpty(updatedPatient.PhoneNumber))
        patient.PhoneNumber = updatedPatient.PhoneNumber;

      if (!string.IsNullOrEmpty(updatedPatient.Email))
        patient.Email = updatedPatient.Email;

      if (!string.IsNullOrEmpty(updatedPatient.EmergencyContact))
        patient.EmergencyContact = updatedPatient.EmergencyContact;

      if (!string.IsNullOrEmpty(updatedPatient.EmergencyPhoneNumber))
        patient.EmergencyPhoneNumber = updatedPatient.EmergencyPhoneNumber;

      await _context.SaveChangesAsync();
      serviceResponse.Data = _mapper.Map<GetPatientDto>(patient);
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

      var Patient = await _context.Patients.FirstOrDefaultAsync(c => c.PatientId == id) ?? throw new Exception($"Patient with Id {id} not found");
      _context.Patients.Remove(Patient);
      await _context.SaveChangesAsync();
      serviceResponse.Data = null;
      serviceResponse.Message = $"Deleted a PatientId {id}";

    }
    catch (Exception ex)
    {
      serviceResponse.Success = false;
      serviceResponse.Message = ex.Message;
    }

    return serviceResponse;

  }


}
