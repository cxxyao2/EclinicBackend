using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services.LabTestService;
using Microsoft.AspNetCore.Mvc;

namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabTestsController : ControllerBase
    {
        private readonly ILabTestService _LabTestService;

        public LabTestsController(ILabTestService LabTestService)
        {
            _LabTestService = LabTestService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetLabTestDto>>>> Get()
        {
            return Ok(await _LabTestService.GetAll());

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetLabTestDto>>> GetSingle(int id)
        {

            return Ok(await _LabTestService.GetById(id));

        }

        // [HttpPost]
        // public async Task<ActionResult<ServiceResponse<GetLabTestDto>>> AddLabTest(AddLabTestDto newLabTest)
        // {


        //     return Ok(await _LabTestService.Add(newLabTest));

        // }


        // [HttpPut]
        // public async Task<ActionResult<ServiceResponse<GetLabTestDto>>> UpdateLabTest(GetLabTestDto updatedLabTest)
        // {
        //     var response = await _LabTestService.Update(updatedLabTest);
        //     if (response.Data is null) return NotFound();

        //     return Ok(response);

        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        // {

        //     var response = await _LabTestService.Delete(id);

        //     return Ok(response);

        // }
    }
}