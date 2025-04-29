using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services.ImageRecordService;
using Microsoft.AspNetCore.Mvc;

namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageRecordsController : ControllerBase
    {
        private readonly string _imageFolderPath = "appdata/images/";
        private readonly IImageRecordService _ImageRecordService;

        public ImageRecordsController(IImageRecordService ImageRecordService)
        {
            _ImageRecordService = ImageRecordService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetImageRecordDto>>>> Get()
        {
            return Ok(await _ImageRecordService.GetAll());
            // https：//xxx?filename=xxxx.   

        }

        [HttpGet("images/{fileName}")]
        public async Task<IActionResult> GetImage(string fileName)
        {
            var filePath = Path.Combine(_imageFolderPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Image not found.");
            }

            var memoryStream = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;

            // Return image as a file result
            return File(memoryStream, "image/jpeg", fileName);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetImageRecordDto>>> GetSingle(int id)
        {

            return Ok(await _ImageRecordService.GetById(id));

        }

        // [HttpPost]
        // public async Task<ActionResult<ServiceResponse<GetImageRecordDto>>> AddImageRecord(AddImageRecordDto newImageRecord)
        // {


        //     return Ok(await _ImageRecordService.Add(newImageRecord));

        // }


        // [HttpPut]
        // public async Task<ActionResult<ServiceResponse<GetImageRecordDto>>> UpdateImageRecord(GetImageRecordDto updatedImageRecord)
        // {
        //     var response = await _ImageRecordService.Update(updatedImageRecord);
        //     if (response.Data is null) return NotFound();

        //     return Ok(response);

        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<ServiceResponse<string>>> Delete(int id)
        // {

        //     var response = await _ImageRecordService.Delete(id);

        //     return Ok(response);

        // }
    }
}