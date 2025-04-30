using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services.PatientService;
using Microsoft.AspNetCore.Mvc;

namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {


        private readonly IPatientService _PatientService;

        public PatientsController(IPatientService PatientService)
        {
            _PatientService = PatientService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetPatientDto>>>> Get()
        {
            return Ok(await _PatientService.GetAll());

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetPatientDto>>> GetSingle(int id)
        {

            return Ok(await _PatientService.GetById(id));

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetPatientDto>>> AddPatient(AddPatientDto newPatient)
        {


            return Ok(await _PatientService.Add(newPatient));

        }


        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetPatientDto>>> UpdatePatient(GetPatientDto updatedPatient)
        {
            var response = await _PatientService.Update(updatedPatient);
            if (response.Data is null) return NotFound();

            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        {

            var response = await _PatientService.Delete(id);

            return Ok(response);

        }
    }
}