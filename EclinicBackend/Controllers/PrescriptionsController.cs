using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services.PrescriptionService;
using Microsoft.AspNetCore.Mvc;

namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {


        private readonly IPrescriptionService _PrescriptionService;

        public PrescriptionsController(IPrescriptionService PrescriptionService)
        {
            _PrescriptionService = PrescriptionService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetPrescriptionDto>>>> Get()
        {
            return Ok(await _PrescriptionService.GetAll());

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetPrescriptionDto>>> GetSingle(int id)
        {

            return Ok(await _PrescriptionService.GetById(id));

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetPrescriptionDto>>> AddPrescription(AddPrescriptionDto newPrescription)
        {


            return Ok(await _PrescriptionService.Add(newPrescription));

        }

        [HttpPost("batch-add")]
        public async Task<ActionResult<ServiceResponse<int>>> BatchAdd(List<AddPrescriptionDto> prescriptions)
        {
            if (prescriptions == null || prescriptions.Count == 0)
            {
                return BadRequest(new ServiceResponse<int>
                {
                    Success = false,
                    Message = "No prescriptions provided",
                    Data = 0
                });
            }

            var response = await _PrescriptionService.BatchAdd(prescriptions);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetPrescriptionDto>>> UpdatePrescription(GetPrescriptionDto updatedPrescription)
        {
            var response = await _PrescriptionService.Update(updatedPrescription);
            if (response.Data is null) return NotFound();

            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        {

            var response = await _PrescriptionService.Delete(id);

            return Ok(response);

        }
    }
}
