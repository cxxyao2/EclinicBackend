using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services;
using EclinicBackend.Services.VisitRecordService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System;

// Deprecated. 2024.Dec.08.
namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitRecordsController : ControllerBase
    {

        private readonly IVisitRecordService _VisitRecordService;
        private readonly IConfiguration _configuration;

        public VisitRecordsController(
            IVisitRecordService VisitRecordService,
            IConfiguration configuration)
        {
            _VisitRecordService = VisitRecordService;
            _configuration = configuration;
        }

        [HttpGet("waiting-list")]
        public async Task<ActionResult<ServiceResponse<List<GetVisitRecordDto>>>> Get([FromQuery] string visitDate)
        {
            if (!string.IsNullOrEmpty(visitDate))
            {
                if (DateTime.TryParse(visitDate,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                out DateTime parsedDate))
                {
                    parsedDate = parsedDate.ToUniversalTime();
                    var response = await _VisitRecordService.GetWaitingList(parsedDate);
                    if (response.Data == null || response.Data.Count == 0)
                        response.Message = "No schedules found for the given date.";
                    return Ok(response);
                }
                return BadRequest(new ServiceResponse<List<GetVisitRecordDto>>
                {
                    Success = false,
                    Message = $"Invalid date format. Use UTC time string format."
                });
            }
            return BadRequest("Visit date is not valid");
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetVisitRecordDto>>>> Get(
            [FromQuery] int? practitionerId,
            [FromQuery] string visitDate,
            [FromQuery] int? patientId)
        {
            if (practitionerId.HasValue && !string.IsNullOrEmpty(visitDate))
            {
                if (DateTime.TryParse(visitDate,
                 CultureInfo.InvariantCulture,
                 DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                 out DateTime parsedDate))
                {
                    parsedDate = parsedDate.ToUniversalTime();
                    var response = await _VisitRecordService.GetByPractitionerAndDate(
                        practitionerId.Value,
                        parsedDate);
                    if (response.Data == null || response.Data.Count == 0)
                        response.Message = "No schedules found for the given PractitionerId and visitDate.";
                    return Ok(response);
                }
                return BadRequest(new ServiceResponse<List<GetVisitRecordDto>>
                {
                    Success = false,
                    Message = $"Invalid date format. Use utc time"
                });
            }

            if (patientId.HasValue)
            {
                var response = await _VisitRecordService.GetByPatient(patientId.Value);
                if (response.Data == null || response.Data.Count == 0)
                    response.Message = "No schedules found for the given PatientId.";
                return Ok(response);
            }

            if (!string.IsNullOrEmpty(visitDate))
            {
                if (DateTime.TryParse(visitDate,
              CultureInfo.InvariantCulture,
              DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
              out DateTime parsedDate))
                {

                    var response = await _VisitRecordService.GetByDate(parsedDate);
                    if (response.Data == null || response.Data.Count == 0)
                        response.Message = "No schedules found for the given date.";
                    return Ok(response);
                }
                return BadRequest(new ServiceResponse<List<GetVisitRecordDto>>
                {
                    Success = false,
                    Message = $"Invalid date format. Use utc time string"
                });
            }

            return Ok(await _VisitRecordService.GetAll());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetVisitRecordDto>>> GetSingle(int id)
        {

            return Ok(await _VisitRecordService.GetById(id));

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetVisitRecordDto>>> AddVisitRecord(AddVisitRecordDto newVisitRecord)
        {


            return Ok(await _VisitRecordService.Add(newVisitRecord));

        }


        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetVisitRecordDto>>> UpdateVisitRecord(GetVisitRecordDto updatedVisitRecord)
        {
            var response = await _VisitRecordService.Update(updatedVisitRecord);
            if (response.Data is null) return NotFound();

            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        {

            var response = await _VisitRecordService.Delete(id);

            return Ok(response);

        }

        [HttpGet("search")]
        public async Task<ActionResult<ServiceResponse<PaginatedResponse<GetMedicalHistoryDto>>>> SearchSimilarCases(
            [FromQuery] string searchTerm,
            [FromQuery] string? startDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term is required");
            }

            DateTime? parsedStartDate = null;
            if (!string.IsNullOrEmpty(startDate))
            {
                if (DateTime.TryParse(startDate,
              CultureInfo.InvariantCulture,
              DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
              out DateTime date))
                {
                    parsedStartDate = date;
                }
                else
                {
                    return BadRequest(new ServiceResponse<PaginatedResponse<GetMedicalHistoryDto>>
                    {
                        Success = false,
                        Message = $"Invalid date format. Use utc time string format."
                    });
                }
            }

            if (page < 1) page = 1;
            if (size < 1) size = 10;
            if (size > 100) size = 100;

            var response = await _VisitRecordService.SearchSimilarCases(
                searchTerm,
                parsedStartDate,
                page,
                size);

            return Ok(response);
        }
    }
}
