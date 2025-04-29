using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using EclinicBackend.Services.PractitionerScheduleService;
using EclinicBackend.Dtos;

namespace EclinicBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PractitionerSchedulesController : ControllerBase
    {
        private readonly IPractitionerScheduleService _practitionerScheduleService;
        private readonly IConfiguration _configuration;

        public PractitionerSchedulesController(
            IPractitionerScheduleService practitionerScheduleService,
            IConfiguration configuration)
        {
            _practitionerScheduleService = practitionerScheduleService;
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetPractitionerScheduleDto>>>> GetAll()
        {
            return Ok(await _practitionerScheduleService.GetAll());
        }

        [HttpGet("by-practitioner-date")]
        public async Task<ActionResult<ServiceResponse<List<GetPractitionerScheduleDto>>>> GetByPractitionerAndDate(
            [FromQuery] int practitionerId,
            [FromQuery] string workDate)
        {
            if (string.IsNullOrEmpty(workDate))
            {
                return BadRequest(new ServiceResponse<List<GetPractitionerScheduleDto>>
                {
                    Success = false,
                    Message = "WorkDate is required"
                });
            }

            // Parse as ISO 8601 UTC format (e.g. "2025-04-21T04:00:00Z")
            if (DateTime.TryParse(workDate,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                out DateTime parsedDate))
            {
                // No need to call ToUniversalTime() as the date is already in UTC format
                var response = await _practitionerScheduleService.GetByPractitionerAndDate(
                    practitionerId,
                    parsedDate);

                if (response.Data == null || response.Data.Count == 0)
                {
                    response.Message = "No schedules found for the given PractitionerId and WorkDate.";
                }
                return Ok(response);
            }

            return BadRequest(new ServiceResponse<List<GetPractitionerScheduleDto>>
            {
                Success = false,
                Message = $"Invalid date format. Use ISO 8601 UTC format (e.g. '2025-04-21T04:00:00Z')"
            });
        }

        [HttpGet("by-patient")]
        public async Task<ActionResult<ServiceResponse<List<GetPractitionerScheduleDto>>>> GetByPatient(
            [FromQuery] int patientId)
        {
            var response = await _practitionerScheduleService.GetByPatient(patientId);
            if (response.Data == null || response.Data.Count == 0)
            {
                response.Message = "No schedules found for the given PatientId.";
            }
            return Ok(response);
        }

        [HttpGet("by-date")]
        public async Task<ActionResult<ServiceResponse<List<GetPractitionerScheduleDto>>>> GetByDate(
            [FromQuery] string workDate)
        {
            if (string.IsNullOrEmpty(workDate))
            {
                return BadRequest(new ServiceResponse<List<GetPractitionerScheduleDto>>
                {
                    Success = false,
                    Message = "WorkDate is required"
                });
            }

            // Parse as ISO 8601 UTC format
            if (DateTime.TryParse(workDate,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                out DateTime parsedDate))
            {
                var response = await _practitionerScheduleService.GetByDate(parsedDate);
                if (response.Data == null || response.Data.Count == 0)
                {
                    response.Message = "No schedules found for the given date.";
                }
                return Ok(response);
            }

            return BadRequest(new ServiceResponse<List<GetPractitionerScheduleDto>>
            {
                Success = false,
                Message = $"Invalid date format. Use ISO 8601 UTC format (e.g. '2025-04-21T04:00:00Z')"
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetPractitionerScheduleDto>>> GetSingle(int id)
        {

            return Ok(await _practitionerScheduleService.GetById(id));

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetPractitionerScheduleDto>>> AddPractitionerSchedule(AddPractitionerScheduleDto newPractitionerSchedule)
        {


            return Ok(await _practitionerScheduleService.Add(newPractitionerSchedule));

        }


        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetPractitionerScheduleDto>>> UpdatePractitionerSchedule(UpdatePractitionerScheduleDto updatedPractitionerSchedule)
        {
            var response = await _practitionerScheduleService.Update(updatedPractitionerSchedule);
            if (response.Data is null) return NotFound();

            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        {

            var response = await _practitionerScheduleService.Delete(id);

            return Ok(response);

        }

        [HttpPost("batch-delete")]
        public async Task<ActionResult<ServiceResponse<string>>> BatchDelete([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest(new ServiceResponse<string>
                {
                    Success = false,
                    Message = "No IDs provided for deletion"
                });
            }

            var response = await _practitionerScheduleService.BatchDelete(ids);
            return Ok(response);
        }

        [HttpPost("batch-add")]
        public async Task<ActionResult<ServiceResponse<List<GetPractitionerScheduleDto>>>> AddBatch(List<AddPractitionerScheduleDto> newSchedules)
        {
            if (newSchedules == null || newSchedules.Count == 0)
            {
                return BadRequest(new ServiceResponse<List<GetPractitionerScheduleDto>>
                {
                    Success = false,
                    Message = "No schedules provided"
                });
            }

            var response = await _practitionerScheduleService.AddBatch(newSchedules);
            return Ok(response);
        }
    }
}
