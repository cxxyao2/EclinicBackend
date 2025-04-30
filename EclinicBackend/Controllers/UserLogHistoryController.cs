using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EclinicBackend.Services.UserLogHistoryService;
using System.Threading.Tasks;
using EclinicBackend.Models;


namespace EclinicBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserLogHistoryController : ControllerBase
    {
        private readonly IUserLogHistoryService _logHistoryService;

        public UserLogHistoryController(IUserLogHistoryService logHistoryService)
        {
            _logHistoryService = logHistoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Restrict to admin users
        public async Task<ActionResult<IEnumerable<UserLogHistory>>> GetAll([FromQuery] int? userId = null)
        {
            var history = await _logHistoryService.GetUserLogHistory(userId);
            return Ok(history);
        }
    }
}