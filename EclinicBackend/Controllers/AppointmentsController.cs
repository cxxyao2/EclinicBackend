using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services.AppointmentService;
using Microsoft.AspNetCore.Mvc;

// Deprecated. 2024.Dec.08.
namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {


        private readonly IAppointmentService _AppointmentService;

        public AppointmentsController(IAppointmentService AppointmentService)
        {
            _AppointmentService = AppointmentService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetAppointmentDto>>>> Get()
        {
            return Ok(await _AppointmentService.GetAll());

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetAppointmentDto>>> GetSingle(int id)
        {

            return Ok(await _AppointmentService.GetById(id));

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetAppointmentDto>>> AddAppointment(AddAppointmentDto newAppointment)
        {


            return Ok(await _AppointmentService.Add(newAppointment));

        }


        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetAppointmentDto>>> UpdateAppointment(GetAppointmentDto updatedAppointment)
        {
            var response = await _AppointmentService.Update(updatedAppointment);
            if (response.Data is null) return NotFound();

            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        {

            var response = await _AppointmentService.Delete(id);

            return Ok(response);

        }
    }
}