using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services.PractitionerService;
using Microsoft.AspNetCore.Mvc;

namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PractitionersController : ControllerBase
    {


        private readonly IPractitionerService _PractitionerService;

        public PractitionersController(IPractitionerService PractitionerService)
        {
            _PractitionerService = PractitionerService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetPractitionerDto>>>> Get()
        {
            return Ok(await _PractitionerService.GetAll());

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetPractitionerDto>>> GetSingle(int id)
        {

            return Ok(await _PractitionerService.GetById(id));

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetPractitionerDto>>> AddPractitioner(AddPractitionerDto newPractitioner)
        {


            return Ok(await _PractitionerService.Add(newPractitioner));

        }


        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetPractitionerDto>>> UpdatePractitioner(GetPractitionerDto updatedPractitioner)
        {
            var response = await _PractitionerService.Update(updatedPractitioner);

            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        {

            var response = await _PractitionerService.Delete(id);

            return Ok(response);

        }
    }
}