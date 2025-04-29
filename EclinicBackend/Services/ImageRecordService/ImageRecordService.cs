using AutoMapper;
using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace EclinicBackend.Services.ImageRecordService;


public class ImageRecordService : IImageRecordService
{
  private readonly IMapper _mapper;
  private readonly ApplicationDbContext _context;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public ImageRecordService(IMapper mapper, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
  {
    _mapper = mapper;
    _context = context;
    _httpContextAccessor = httpContextAccessor;
  }

  // private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

  // public async Task<ServiceResponse<GetImageRecordDto>> Add(AddImageRecordDto newImageRecord)
  // {
  //     var serviceResponse = new ServiceResponse<GetImageRecordDto>();
  //     var ImageRecord = _mapper.Map<ImageRecord>(newImageRecord);

  //     _context.ImageRecords.Add(ImageRecord);
  //     await _context.SaveChangesAsync();

  //     serviceResponse.Data = _mapper.Map<GetImageRecordDto>(ImageRecord);

  //     return serviceResponse;
  // }


  public async Task<ServiceResponse<List<GetImageRecordDto>>> GetAll()
  {
    var serviceResponse = new ServiceResponse<List<GetImageRecordDto>>();
    var dbImageRecords = await _context.ImageRecords
    .Include(x => x.Patient)
    .Include(x => x.Practitioner)
    .Select(x => new GetImageRecordDto
    {
      ImageId = x.ImageId,
      PatientId = x.PatientId,
      PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
      PractitionerId = x.PractitionerId,
      PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
      InpatientId = x.InpatientId,
      ImageType = x.ImageType,
      ImageDescription = x.ImageDescription,
      ImagePath = x.ImagePath,
      CaptureDate = x.CaptureDate,
      Status = x.Status,
    })
      .ToListAsync();
    serviceResponse.Data = dbImageRecords;
    return serviceResponse;
  }

  public async Task<ServiceResponse<GetImageRecordDto>> GetById(int Id)
  {
    var serviceResponse = new ServiceResponse<GetImageRecordDto>();
    var dbImageRecord = await _context.ImageRecords
      .Include(x => x.Patient)
      .Include(x => x.Practitioner)
      .Select(x => new GetImageRecordDto
      {
        ImageId = x.ImageId,
        PatientId = x.PatientId,
        PatientName = (x.Patient != null) ? (x.Patient.FirstName + " " + x.Patient.LastName) : "",
        PractitionerId = x.PractitionerId,
        PractitionerName = (x.Practitioner != null) ? (x.Practitioner.FirstName + " " + x.Practitioner.LastName) : "",
        InpatientId = x.InpatientId,
        ImageType = x.ImageType,
        ImageDescription = x.ImageDescription,
        ImagePath = x.ImagePath,
        CaptureDate = x.CaptureDate,
        Status = x.Status,
      })
      .FirstOrDefaultAsync(c => c.ImageId == Id);
    serviceResponse.Data = dbImageRecord;

    return serviceResponse;
  }

  // public async Task<ServiceResponse<GetImageRecordDto>> Update(GetImageRecordDto updatedImageRecord)
  // {
  //     var serviceResponse = new ServiceResponse<GetImageRecordDto>();
  //     try
  //     {
  //         var ImageRecord = await _context.ImageRecords
  //           .FirstOrDefaultAsync(c => c.ImageRecordId == updatedImageRecord.ImageRecordId);
  //         if (ImageRecord is null)
  //             throw new Exception($"ImageRecord {updatedImageRecord.ImageRecordId} not found");


  //         // Update only non-null fields
  //         if (!string.IsNullOrEmpty(updatedImageRecord.ReasonForVisit))
  //             ImageRecord.ReasonForVisit = updatedImageRecord.ReasonForVisit;

  //         if (updatedImageRecord.Status != Enums.ImageRecordStatusEnum.Scheduled)
  //             ImageRecord.Status = updatedImageRecord.Status;

  //         await _context.SaveChangesAsync();
  //         serviceResponse.Data = _mapper.Map<GetImageRecordDto>(ImageRecord);
  //     }
  //     catch (Exception ex)
  //     {
  //         serviceResponse.Data = null;
  //         serviceResponse.Success = false;
  //         serviceResponse.Message = ex.Message;
  //     }

  //     return serviceResponse;

  // }



  // public async Task<ServiceResponse<string>> Delete(int Id)
  // {
  //     var serviceResponse = new ServiceResponse<string>();

  //     try
  //     {

  //         var ImageRecord = await _context.ImageRecords.FirstOrDefaultAsync(c => c.ImageRecordId == Id) ?? throw new Exception($"ImageRecord with Id {Id} not found");
  //         _context.ImageRecords.Remove(ImageRecord);
  //         await _context.SaveChangesAsync();
  //         serviceResponse.Data = null;
  //         serviceResponse.Message = $"Deleted a ImageRecordId {Id}";

  //     }
  //     catch (Exception ex)
  //     {
  //         serviceResponse.Success = false;
  //         serviceResponse.Message = ex.Message;
  //     }

  //     return serviceResponse;

  // }


}
