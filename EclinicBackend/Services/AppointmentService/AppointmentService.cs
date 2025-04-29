using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.AppointmentService;

// Deprecated.2024.Dec.7: This table will be deleted possibly.
public class AppointmentService : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppointmentService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<ServiceResponse<GetAppointmentDto>> Add(AddAppointmentDto newAppointment)
    {
        var serviceResponse = new ServiceResponse<GetAppointmentDto>();
        var Appointment = _mapper.Map<Appointment>(newAppointment);

        _context.Appointments.Add(Appointment);
        await _context.SaveChangesAsync();

        serviceResponse.Data = _mapper.Map<GetAppointmentDto>(Appointment);

        return serviceResponse;
    }


    public async Task<ServiceResponse<List<GetAppointmentDto>>> GetAll()
    {
        var serviceResponse = new ServiceResponse<List<GetAppointmentDto>>();
        var dbAppointments = await _context.Appointments
        .Include(x => x.Patient)
        .Select(x => new GetAppointmentDto
        {
            AppointmentId = x.AppointmentId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + "" + x.Patient.LastName) : "",
            AvailableId = x.AvailableId,
            ReasonForVisit = x.ReasonForVisit,

        })
          .ToListAsync();
        serviceResponse.Data = dbAppointments;
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetAppointmentDto>> GetById(int id)
    {
        var serviceResponse = new ServiceResponse<GetAppointmentDto>();
        var dbAppointment = await _context.Appointments
        .Include(x => x.Patient)
        .Select(x => new GetAppointmentDto
        {
            AppointmentId = x.AppointmentId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + "" + x.Patient.LastName) : "",
            AvailableId = x.AvailableId,
            ReasonForVisit = x.ReasonForVisit,

        })
          .FirstOrDefaultAsync(c => c.AppointmentId == id);
        serviceResponse.Data = _mapper.Map<GetAppointmentDto>(dbAppointment);

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetAppointmentDto>> Update(GetAppointmentDto updatedAppointment)
    {
        var serviceResponse = new ServiceResponse<GetAppointmentDto>();
        try
        {
            var appointment = await _context.Appointments
              .FirstOrDefaultAsync(c => c.AppointmentId == updatedAppointment.AppointmentId);
            if (appointment is null)
                throw new Exception($"Appointment {updatedAppointment.AppointmentId} not found");


            // Update only non-null fields
            if (!string.IsNullOrEmpty(updatedAppointment.ReasonForVisit))
                appointment.ReasonForVisit = updatedAppointment.ReasonForVisit;

            if (updatedAppointment.Status != Enums.AppointmentStatusEnum.Scheduled)
                appointment.Status = updatedAppointment.Status;

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetAppointmentDto>(appointment);
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

            var Appointment = await _context.Appointments.FirstOrDefaultAsync(c => c.AppointmentId == id) ?? throw new Exception($"Appointment with Id {id} not found");
            _context.Appointments.Remove(Appointment);
            await _context.SaveChangesAsync();
            serviceResponse.Data = null;
            serviceResponse.Message = $"Deleted a AppointmentId {id}";

        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }

        return serviceResponse;

    }


}
