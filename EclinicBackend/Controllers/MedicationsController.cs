using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services.MedicationService;
using Microsoft.AspNetCore.Mvc;

namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private readonly IMedicationService _MedicationService;

        public MedicationsController(IMedicationService MedicationService)
        {
            _MedicationService = MedicationService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetMedicationDto>>>> Get()
        {
            return Ok(await _MedicationService.GetAll());

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetMedicationDto>>> GetSingle(int id)
        {

            return Ok(await _MedicationService.GetById(id));

        }

        // [HttpPost]
        // public async Task<ActionResult<ServiceResponse<GetMedicationDto>>> AddMedication(AddMedicationDto newMedication)
        // {


        //     return Ok(await _MedicationService.Add(newMedication));

        // }


        // [HttpPut]
        // public async Task<ActionResult<ServiceResponse<GetMedicationDto>>> UpdateMedication(GetMedicationDto updatedMedication)
        // {
        //     var response = await _MedicationService.Update(updatedMedication);
        //     if (response.Data is null) return NotFound();

        //     return Ok(response);

        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        // {

        //     var response = await _MedicationService.Delete(id);

        //     return Ok(response);

        // }
    }
}