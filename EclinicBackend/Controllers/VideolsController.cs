using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EclinicBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {

            // todo
            var videos = new List<string> { "summer1.mp4", "summer2.mp4" };
            return Ok(videos);
        }
    }
}
