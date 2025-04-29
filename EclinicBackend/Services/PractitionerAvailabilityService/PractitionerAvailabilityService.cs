
using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.PractitionerAvailabilityService;

// Deprecated. 2024.Dec.7
public class PractitionerAvailabilityService : IPractitionerAvailabilityService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PractitionerAvailabilityService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<ServiceResponse<GetPractitionerAvailabilityDto>> Add(AddPractitionerAvailabilityDto newEntity)
    {
        var serviceResponse = new ServiceResponse<GetPractitionerAvailabilityDto>();
        var newAvailable = _mapper.Map<PractitionerAvailability>(newEntity);

        _context.PractitionerAvailabilities.Add(newAvailable);
        await _context.SaveChangesAsync();

        var available = await _context.PractitionerAvailabilities
       .Include(b => b.Practitioner) // Include Author data
       .Where(b => b.PractitionerId == newAvailable.PractitionerId)
       .Select(b => new GetPractitionerAvailabilityDto
       {
           AvailableId = b.AvailableId,
           PractitionerId = b.PractitionerId,
           PractitionerName = b.Practitioner != null
            ? (b.Practitioner.FirstName + " " + b.Practitioner.LastName)
            : "",
           SlotDateTime = b.SlotDateTime,
           IsAvailable = b.IsAvailable,
       })
         .FirstOrDefaultAsync(c => c.AvailableId == newAvailable.AvailableId);

        serviceResponse.Data = available;
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetPractitionerAvailabilityDto>>> GetAll()
    {
        var serviceResponse = new ServiceResponse<List<GetPractitionerAvailabilityDto>>();
        var dbPractitionerAvailabilitys = await _context.PractitionerAvailabilities
         .Include(c => c.Practitioner)
            .Select(c => new GetPractitionerAvailabilityDto
            {
                AvailableId = c.AvailableId,
                SlotDateTime = c.SlotDateTime,
                IsAvailable = c.IsAvailable,
                PractitionerId = c.PractitionerId,
                PractitionerName = c.Practitioner != null ? c.Practitioner.FirstName + ' ' + c.Practitioner.LastName : ""
            })
          .ToListAsync();
        serviceResponse.Data = dbPractitionerAvailabilitys.Select(c => _mapper.Map<GetPractitionerAvailabilityDto>(c)).ToList();
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetPractitionerAvailabilityDto>> GetById(int id)
    {
        var serviceResponse = new ServiceResponse<GetPractitionerAvailabilityDto>();

        var dbPractitionerAvailability = await _context.PractitionerAvailabilities
            .Include(c => c.Practitioner)
            .Where(c => c.AvailableId == id)
            .Select(c => new GetPractitionerAvailabilityDto
            {
                AvailableId = c.AvailableId,
                SlotDateTime = c.SlotDateTime,
                IsAvailable = c.IsAvailable,
                PractitionerId = c.PractitionerId,
                PractitionerName = c.Practitioner != null ? c.Practitioner.FirstName + ' ' + c.Practitioner.LastName : ""
            })
            .FirstOrDefaultAsync();
        serviceResponse.Data = dbPractitionerAvailability;
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetPractitionerAvailabilityDto>> Update(GetPractitionerAvailabilityDto updatedPractitionerAvailability)
    {
        var serviceResponse = new ServiceResponse<GetPractitionerAvailabilityDto>();
        try
        {
            var availability = await _context.PractitionerAvailabilities
              .FirstOrDefaultAsync(c => c.AvailableId == updatedPractitionerAvailability.AvailableId);
            if (availability is null)
                throw new Exception($"PractitionerAvailability {updatedPractitionerAvailability.AvailableId} not found");

            // only IsAvailable can be updated.
            availability.IsAvailable = updatedPractitionerAvailability.IsAvailable;
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetPractitionerAvailabilityDto>(availability);
        }
        catch (Exception ex)
        {

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

            var PractitionerAvailability = await _context.PractitionerAvailabilities.FirstOrDefaultAsync(c => c.AvailableId == id) ?? throw new Exception($"PractitionerAvailability with Id {id} not found");
            _context.PractitionerAvailabilities.Remove(PractitionerAvailability);
            await _context.SaveChangesAsync();

            serviceResponse.Data = "Deleted an Availability.";
        }
        catch (Exception ex)
        {
            serviceResponse.Data = "";
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        return serviceResponse;
    }


}
