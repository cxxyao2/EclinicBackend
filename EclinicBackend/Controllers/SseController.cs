using EclinicBackend.Services.InpatientService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace EclinicBackend.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class SseController : ControllerBase
{
    private readonly IInpatientService _inpatientService;

    public SseController(IInpatientService inpatientService)
    {
        _inpatientService = inpatientService;
    }


    [HttpGet("sse-endpoint")]
    public async Task SseEndpoint()
    {
        Response.Headers["Content-Type"] = "text/event-stream";
        Response.Headers["Cache-Control"] = "no-cache";
        Response.Headers["Connection"] = "keep-alive";

        // StreamWriter for sending SSE messages
        var streamWriter = new StreamWriter(Response.Body);
        // int counter = 0;

        while (!HttpContext.RequestAborted.IsCancellationRequested)
        {
            var inpatients = await _inpatientService.GetPatientsWithoutNurse();
            var json = JsonSerializer.Serialize(inpatients);
            await streamWriter.WriteLineAsync($"data: {json}\n");
            await streamWriter.FlushAsync();

            // Wait for 3 second before sending the next message
            await Task.Delay(3000);
        }
    }
}
