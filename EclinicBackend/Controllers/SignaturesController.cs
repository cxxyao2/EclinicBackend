using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using EclinicBackend.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using EclinicBackend.Models;
using EclinicBackend.Dtos;

namespace EclinicBackend.Controllers;


[Route("api/[Controller]")]
[ApiController]
public class SignaturesController : Controller
{
    // private readonly string _uploadPath = "wwwroot/uploads/";
    private readonly string _uploadPath = "appdata/signatures/";
    private readonly ApplicationDbContext _dbContext;

    public SignaturesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext; // Inject DbContext
    }


    [HttpGet]
    public async Task<ActionResult<ServiceResponse<string>>> GetSignature(int visitRecordId)
    {
        var serviceResponse = new ServiceResponse<string>();

        try
        {
            var visitRecord = await _dbContext.VisitRecords
                .FirstOrDefaultAsync(hr => hr.VisitId == visitRecordId);

            if (visitRecord != null && !string.IsNullOrEmpty(visitRecord.PractitionerSignaturePath))
            {
                string filePath = visitRecord.PractitionerSignaturePath;

                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    string base64String = Convert.ToBase64String(fileBytes);
                    serviceResponse.Data = "data:image/png;base64," + base64String;
                    serviceResponse.Message = "Signature retrieved successfully";
                    serviceResponse.Success = true;

                }
                else
                {
                    serviceResponse.Message = "Signature not found";
                    serviceResponse.Data = "";
                    serviceResponse.Success = false;
                }


                serviceResponse.Success = true;
            }
            else
            {
                serviceResponse.Message = "No signature found";
                serviceResponse.Data = "";
                serviceResponse.Success = false;
            }
        }
        catch (Exception ex)
        {
            serviceResponse.Message = ex.Message;
            serviceResponse.Data = "";
            serviceResponse.Success = false;
        }

        return Ok(serviceResponse);
    }


    [HttpPost]
    public async Task<ActionResult<ServiceResponse<string>>> SaveSignature([FromBody] SignatureDto model)
    {
        var serviceResponse = new ServiceResponse<string>();

        try
        {
            // Ensure the base64 string is valid
            if (string.IsNullOrEmpty(model.Image) || !Regex.IsMatch(model.Image, @"^data:image\/[a-zA-Z]+;base64,"))
            {
                throw new Exception("Invalid image data.");
            }


            // Ensure the uploads directory exists
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }

            // Decode the base64 string to get the byte array
            var base64Data = Regex.Match(model.Image, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            byte[] imageBytes;

            try
            {
                imageBytes = Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new Exception("Failed to decode image. Invalid base64 string.");
            }

            // Create a unique filename and save the image to the server
            string fileName = Guid.NewGuid().ToString() + ".png";
            string filePath = Path.Combine(_uploadPath, fileName);

            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
            var existingRecord = await _dbContext.VisitRecords
                .FirstOrDefaultAsync(hr => hr.VisitId == model.VisitRecordId);
            if (existingRecord != null)
            {
                existingRecord.PractitionerSignaturePath = fileName;
                _dbContext.VisitRecords.Update(existingRecord);
            }
            else
            {
                throw new Exception("No visit found");

            }

            await _dbContext.SaveChangesAsync();

            serviceResponse.Message = "Signature saved successfully";
            serviceResponse.Data = filePath;
            serviceResponse.Success = true;
        }
        catch (Exception ex)
        {
            serviceResponse.Message = ex.Message;
            serviceResponse.Data = "";
            serviceResponse.Success = false;
        }


        return Ok(serviceResponse);
    }
}

