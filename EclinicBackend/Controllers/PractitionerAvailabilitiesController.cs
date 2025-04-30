using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services.PractitionerAvailabilityService;
using Microsoft.AspNetCore.Mvc;

// Deprecated. 2024.Dec.08.
namespace EclinicBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PractitionerAvailabilitiesController : ControllerBase
    {

        private readonly IPractitionerAvailabilityService _PractitionerAvailabilityService;

        public PractitionerAvailabilitiesController(IPractitionerAvailabilityService PractitionerAvailabilityService)
        {
            _PractitionerAvailabilityService = PractitionerAvailabilityService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetPractitionerAvailabilityDto>>>> Get()
        {
            return Ok(await _PractitionerAvailabilityService.GetAll());

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetPractitionerAvailabilityDto>>> GetSingle(int id)
        {

            return Ok(await _PractitionerAvailabilityService.GetById(id));

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetPractitionerAvailabilityDto>>> AddPractitionerAvailability(AddPractitionerAvailabilityDto newPractitionerAvailability)
        {

            return Ok(await _PractitionerAvailabilityService.Add(newPractitionerAvailability));

        }


        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetPractitionerAvailabilityDto>>> UpdatePractitionerAvailability(GetPractitionerAvailabilityDto updatedPractitionerAvailability)
        {
            var response = await _PractitionerAvailabilityService.Update(updatedPractitionerAvailability);
            if (response.Data is null) return NotFound();

            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        {

            var response = await _PractitionerAvailabilityService.Delete(id);

            return Ok(response);

        }
    }
}