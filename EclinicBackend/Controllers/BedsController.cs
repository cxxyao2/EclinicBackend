using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services.BedService;
using Microsoft.AspNetCore.Mvc;

namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedsController : ControllerBase
    {


        private readonly IBedService _BedService;

        public BedsController(IBedService BedService)
        {
            _BedService = BedService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetBedDto>>>> Get()
        {
            return Ok(await _BedService.GetAll());

        }


        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetBedDto>>> UpdateBed(UpdateBedDto updatedBed)
        {
            var response = await _BedService.Update(updatedBed);
            if (response.Data is null) return NotFound();

            return Ok(response);

        }

        [HttpPut("batch-update")]
        public async Task<ActionResult<ServiceResponse<List<UpdateBedDto>>>> BatchUpdateBeds([FromBody] List<UpdateBedDto> updatedBeds)
        {
            if (updatedBeds == null || updatedBeds.Count == 0)
            {
                return BadRequest(new ServiceResponse<List<UpdateBedDto>>
                {
                    Success = false,
                    Message = "No beds provided for update"
                });
            }

            var response = await _BedService.BatchUpdate(updatedBeds);
            if (response.Data == null) return NotFound();

            return Ok(response);
        }

    }
}
