using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.LabTestService;


public class LabTestService : ILabTestService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LabTestService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // public async Task<ServiceResponse<GetLabTestDto>> Add(AddLabTestDto newLabTest)
    // {
    //     var serviceResponse = new ServiceResponse<GetLabTestDto>();
    //     var LabTest = _mapper.Map<LabTest>(newLabTest);

    //     _context.LabTests.Add(LabTest);
    //     await _context.SaveChangesAsync();

    //     serviceResponse.Data = _mapper.Map<GetLabTestDto>(LabTest);

    //     return serviceResponse;
    // }


    public async Task<ServiceResponse<List<GetLabTestDto>>> GetAll()
    {
        var serviceResponse = new ServiceResponse<List<GetLabTestDto>>();
        var dbLabTests = await _context.LabTests
        .Include(x => x.Patient)
        .Include(x => x.Practitioner)
        .Select(x => new GetLabTestDto
        {
            LabTestId = x.LabTestId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            TestName = x.TestName,
            TestResult = x.TestResult,
            TestDate = x.TestDate
        })
          .ToListAsync();
        serviceResponse.Data = dbLabTests;
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetLabTestDto>> GetById(int id)
    {
        var serviceResponse = new ServiceResponse<GetLabTestDto>();
        var dbLabTest = await _context.LabTests
        .Select(x => new GetLabTestDto
        {
            LabTestId = x.LabTestId,
            PatientId = x.PatientId,
            PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
            PractitionerId = x.PractitionerId,
            PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
            TestName = x.TestName,
            TestResult = x.TestResult,
            TestDate = x.TestDate
        })
          .FirstOrDefaultAsync(c => c.LabTestId == id);
        serviceResponse.Data = dbLabTest;

        return serviceResponse;
    }

    // public async Task<ServiceResponse<GetLabTestDto>> Update(GetLabTestDto updatedLabTest)
    // {
    //     var serviceResponse = new ServiceResponse<GetLabTestDto>();
    //     try
    //     {
    //         var LabTest = await _context.LabTests
    //           .FirstOrDefaultAsync(c => c.LabTestId == updatedLabTest.LabTestId);
    //         if (LabTest is null)
    //             throw new Exception($"LabTest {updatedLabTest.LabTestId} not found");


    //         // Update only non-null fields
    //         if (!string.IsNullOrEmpty(updatedLabTest.ReasonForVisit))
    //             LabTest.ReasonForVisit = updatedLabTest.ReasonForVisit;

    //         if (updatedLabTest.Status != Enums.LabTestStatusEnum.Scheduled)
    //             LabTest.Status = updatedLabTest.Status;

    //         await _context.SaveChangesAsync();
    //         serviceResponse.Data = _mapper.Map<GetLabTestDto>(LabTest);
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

    //         var LabTest = await _context.LabTests.FirstOrDefaultAsync(c => c.LabTestId == id) ?? throw new Exception($"LabTest with Id {id} not found");
    //         _context.LabTests.Remove(LabTest);
    //         await _context.SaveChangesAsync();
    //         serviceResponse.Data = null;
    //         serviceResponse.Message = $"Deleted a LabTestId {id}";

    //     }
    //     catch (Exception ex)
    //     {
    //         serviceResponse.Success = false;
    //         serviceResponse.Message = ex.Message;
    //     }

    //     return serviceResponse;

    // }


}
