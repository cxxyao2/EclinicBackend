using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services.InpatientService;
using Microsoft.AspNetCore.Mvc;

namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InpatientsController : ControllerBase
    {
        private readonly IInpatientService _InpatientService;

        public InpatientsController(IInpatientService InpatientService)
        {
            _InpatientService = InpatientService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetInpatientDto>>>> Get([FromQuery] string? roomNumber)
        {
            if (!string.IsNullOrEmpty(roomNumber))
            {
                // GET https://example.com/api/inpatients?roomNumber=204
                return Ok(await _InpatientService.GetInpatientsByRoomNumber(roomNumber));
            }
            else
            {
                // Execute the service method without filtering by room number
                return Ok(await _InpatientService.GetAll());
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetInpatientDto>>> GetSingle(int id)
        {

            return Ok(await _InpatientService.GetById(id));

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetInpatientDto>>> AddInpatient(AddInpatientDto newInpatient)
        {


            return Ok(await _InpatientService.Add(newInpatient));

        }


        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetInpatientDto>>> UpdateInpatient(GetInpatientDto updatedInpatient)
        {
            var response = await _InpatientService.Update(updatedInpatient);
            if (response.Data is null) return NotFound();

            return Ok(response);

        }

        [HttpPut("batch-update")]
        public async Task<ActionResult<ServiceResponse<List<GetInpatientDto>>>> BatchUpdateInpatients([FromBody] List<GetInpatientDto> updatedInpatients)
        {
            if (updatedInpatients == null || updatedInpatients.Count == 0)
            {
                return BadRequest(new ServiceResponse<List<GetInpatientDto>>
                {
                    Success = false,
                    Message = "No inpatients provided for update"
                });
            }

            var response = await _InpatientService.BatchUpdate(updatedInpatients);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        {

            var response = await _InpatientService.Delete(id);

            return Ok(response);

        }
    }
}
