
using System.Security.Claims;
using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.PrescriptionService;
public class PrescriptionService : IPrescriptionService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PrescriptionService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<ServiceResponse<GetPrescriptionDto>> Add(AddPrescriptionDto newEntity)
    {
        var serviceResponse = new ServiceResponse<GetPrescriptionDto>();
        var newPrescription = _mapper.Map<Prescription>(newEntity);
        newPrescription.CreatedBy = GetUserId();
        newPrescription.CreatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var prescription = await this.GetPrescriptionById(newPrescription.PrescriptionId);

        serviceResponse.Data = prescription;
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetPrescriptionDto>>> GetAll()
    {
        var serviceResponse = new ServiceResponse<List<GetPrescriptionDto>>();
        var prescriptions = await _context.Prescriptions
      .Include(b => b.Practitioner)
      .Include(b => b.Medication)
      .Select(b => new GetPrescriptionDto
      {
          VisitId = b.VisitId,
          PrescriptionId = b.PrescriptionId,
          PractitionerId = b.PractitionerId,
          PractitionerName = b.Practitioner != null
           ? (b.Practitioner.FirstName + " " + b.Practitioner.LastName)
           : "",
          MedicationId = b.MedicationId,
          MedicationName = b.Medication != null
           ? b.Medication.Name
           : "",
          StartDate = b.StartDate,
          EndDate = b.EndDate,
      })
         .ToListAsync();
        serviceResponse.Data = prescriptions;
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetPrescriptionDto>> GetById(int Id)
    {
        var serviceResponse = new ServiceResponse<GetPrescriptionDto>();

        var prescription = await this.GetPrescriptionById(Id);
        serviceResponse.Data = prescription;
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetPrescriptionDto>> Update(GetPrescriptionDto updatedPrescription)
    {
        var serviceResponse = new ServiceResponse<GetPrescriptionDto>();
        try
        {
            var prescription = await _context.Prescriptions
              .FirstOrDefaultAsync(c => c.PrescriptionId == updatedPrescription.PrescriptionId);
            if (prescription is null)
                throw new Exception($"Prescription {updatedPrescription.PrescriptionId} not found");


            prescription.Dosage = updatedPrescription.Dosage;
            prescription.Notes = updatedPrescription.Notes;
            prescription.StartDate = updatedPrescription.StartDate;
            prescription.EndDate = updatedPrescription.EndDate;

            prescription.UpdatedBy = GetUserId();
            prescription.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetPrescriptionDto>(prescription);
        }
        catch (Exception ex)
        {

            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;

    }

    public async Task<ServiceResponse<string>> Delete(int Id)
    {
        var serviceResponse = new ServiceResponse<string>();

        try
        {

            var prescription = await _context.PractitionerAvailabilities.FirstOrDefaultAsync(c => c.AvailableId == Id) ?? throw new Exception($"Prescription with Id {Id} not found");
            _context.PractitionerAvailabilities.Remove(prescription);
            await _context.SaveChangesAsync();

            serviceResponse.Data = "Deleted an prescription.";
        }
        catch (Exception ex)
        {
            serviceResponse.Data = "";
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        return serviceResponse;
    }


    public async Task<GetPrescriptionDto?> GetPrescriptionById(int Id)
    {

        var prescription = await _context.Prescriptions
        .Include(b => b.Practitioner)
        .Include(b => b.Medication)
        .Where(b => b.PractitionerId == Id)
        .Select(b => new GetPrescriptionDto
        {
            VisitId = b.VisitId,
            PrescriptionId = b.PrescriptionId,
            PractitionerId = b.PractitionerId,
            PractitionerName = b.Practitioner != null
             ? (b.Practitioner.FirstName + " " + b.Practitioner.LastName)
             : "",
            MedicationId = b.MedicationId,
            MedicationName = b.Medication != null
             ? b.Medication.Name
             : "",
            StartDate = b.StartDate,
            EndDate = b.EndDate,
        })
          .FirstOrDefaultAsync();
        return prescription;
    }

    public async Task<ServiceResponse<int>> BatchAdd(List<AddPrescriptionDto> prescriptions)
    {
        var serviceResponse = new ServiceResponse<int>();

        try
        {
            if (prescriptions == null || prescriptions.Count == 0)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No prescriptions provided";
                serviceResponse.Data = 0;
                return serviceResponse;
            }

            var newPrescriptions = prescriptions.Select(Dto =>
            {
                var prescription = _mapper.Map<Prescription>(Dto);
                prescription.CreatedAt = DateTime.UtcNow;
                prescription.CreatedBy = GetUserId();
                return prescription;
            }).ToList();

            _context.Prescriptions.AddRange(newPrescriptions);
            int rowsAffected = await _context.SaveChangesAsync();

            serviceResponse.Success = true;
            serviceResponse.Data = rowsAffected;
            serviceResponse.Message = $"Successfully added {rowsAffected} prescription records";
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
            serviceResponse.Data = 0;
        }

        return serviceResponse;
    }
}
