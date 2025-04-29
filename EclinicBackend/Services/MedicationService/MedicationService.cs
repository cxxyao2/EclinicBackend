using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.MedicationService;
public class MedicationService : IMedicationService
{
  private readonly IMapper _mapper;
  private readonly ApplicationDbContext _context;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public MedicationService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
  {
    _mapper = mapper;
    _context = context;
    _httpContextAccessor = httpContextAccessor;
  }

  // private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

  // public async Task<ServiceResponse<GetMedicationDto>> Add(AddMedicationDto newMedication)
  // {
  //     var serviceResponse = new ServiceResponse<GetMedicationDto>();
  //     var Medication = _mapper.Map<Medication>(newMedication);

  //     _context.Medications.Add(Medication);
  //     await _context.SaveChangesAsync();

  //     serviceResponse.Data = _mapper.Map<GetMedicationDto>(Medication);

  //     return serviceResponse;
  // }


  public async Task<ServiceResponse<List<GetMedicationDto>>> GetAll()
  {
    var serviceResponse = new ServiceResponse<List<GetMedicationDto>>();
    var dbMedications = await _context.Medications
      .ToListAsync();
    serviceResponse.Data = dbMedications.Select(c => _mapper.Map<GetMedicationDto>(c)).ToList();
    return serviceResponse;
  }

  public async Task<ServiceResponse<GetMedicationDto>> GetById(int id)
  {
    var serviceResponse = new ServiceResponse<GetMedicationDto>();
    var dbMedication = await _context.Medications
      .FirstOrDefaultAsync(c => c.MedicationId == id);
    serviceResponse.Data = _mapper.Map<GetMedicationDto>(dbMedication);

    return serviceResponse;
  }

  // public async Task<ServiceResponse<GetMedicationDto>> Update(GetMedicationDto updatedMedication)
  // {
  //     var serviceResponse = new ServiceResponse<GetMedicationDto>();
  //     try
  //     {
  //         var Medication = await _context.Medications
  //           .FirstOrDefaultAsync(c => c.MedicationId == updatedMedication.MedicationId);
  //         if (Medication is null)
  //             throw new Exception($"Medication {updatedMedication.MedicationId} not found");


  //         // Update only NurserId, DischargeDate, RoomNumber, BedNumber
  //         if (updatedMedication.NurseId != null)
  //             Medication.NurseId = updatedMedication.NurseId;

  //         if (updatedMedication.DischargeDate != null)
  //             Medication.DischargeDate = updatedMedication.DischargeDate;

  //          if (updatedMedication.RoomNumber != null)
  //             Medication.RoomNumber = updatedMedication.RoomNumber;


  //         await _context.SaveChangesAsync();
  //         serviceResponse.Data = _mapper.Map<GetMedicationDto>(Medication);
  //     }
  //     catch (Exception ex)
  //     {
  //         serviceResponse.Data = null;
  //         serviceResponse.Success = false;
  //         serviceResponse.Message = ex.Message;
  //     }

  //     return serviceResponse;

  // }



  // public async Task<ServiceResponse<string>> Delete(int id)
  // {
  //     var serviceResponse = new ServiceResponse<string>();

  //     try
  //     {

  //         var Medication = await _context.Medications.FirstOrDefaultAsync(c => c.MedicationId == id) ?? throw new Exception($"Medication with Id {id} not found");
  //         _context.Medications.Remove(Medication);
  //         await _context.SaveChangesAsync();
  //         serviceResponse.Data = null;
  //         serviceResponse.Message = $"Deleted a MedicationId {id}";

  //     }
  //     catch (Exception ex)
  //     {
  //         serviceResponse.Success = false;
  //         serviceResponse.Message = ex.Message;
  //     }

  //     return serviceResponse;

  // }


}
