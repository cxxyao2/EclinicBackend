using System.ComponentModel.DataAnnotations;
using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EclinicBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;


        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateDto user)
        {
            await _userService.Update(user);
            return Ok(new { message = $"User {user.UserName} updated" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.Delete(id);
            return Ok(new { message = $"User {id} deleted" });
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll([FromQuery] string? email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                User? user = await _userService.GetByEmail(email);
                if (user == null)
                    return NotFound(new { message = $"User {email} not found" });


                return Ok(new List<User> { user });

            }

            IEnumerable<User> users = await _userService.GetAll();

            return Ok(users);
        }






        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            User? user = await _userService.GetById(id);
            if (user == null)
                return NotFound(new { message = $"User {id} not found" });


            return Ok(new { data = user });
        }


    }
}