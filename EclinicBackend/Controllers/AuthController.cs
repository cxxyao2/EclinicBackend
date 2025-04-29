using EclinicBackend.Dtos;
using EclinicBackend.Models;
using EclinicBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EclinicBackend.Services.UserLogHistoryService;
using EclinicBackend.Services.AuthService;

namespace EclinicBackend.Controllers
{
    /// <summary>
    /// Controller for handling authentication-related operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserLogHistoryService _logHistoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(
            IAuthService authService,
            IUserLogHistoryService logHistoryService,
            IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _logHistoryService = logHistoryService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="user">User registration details</param>
        /// <returns>Success message with username</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto>> Register([FromBody] UserCreateDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.Register(user);
            return Ok(new ResponseDto { Message = $"User {user.UserName} created" });
        }

        /// <summary>
        /// Authenticates a user and returns access token
        /// </summary>
        /// <param name="user">Login credentials</param>
        /// <returns>Access token and user details</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] UserLoginDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TokenPair tokenPair = await _authService.Login(user.Email, user.Password);

            // Log the login
            var ipAddress = _httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
                           _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            await _logHistoryService.CreateLoginRecord(
                tokenPair.user.UserID,
                tokenPair.user.UserName,
                ipAddress
            );

            if (tokenPair.RefreshToken != null)
                SetRefreshToken(tokenPair.RefreshToken);

            return Ok(new LoginResponseDto
            {
                AccessToken = tokenPair.AccessToken,
                User = tokenPair.user
            });
        }

        /// <summary>
        /// Refreshes the access token using a refresh token
        /// </summary>
        /// <returns>New token pair</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenPair>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new ResponseDto { Message = "Refresh token is required" });

            var tokenPair = await _authService.RefreshToken(refreshToken);
            if (tokenPair.RefreshToken != null)
                SetRefreshToken(tokenPair.RefreshToken);

            return Ok(tokenPair);
        }

        /// <summary>
        /// Activates a user account using activation code
        /// </summary>
        /// <param name="model">Activation code</param>
        /// <returns>Success message</returns>
        [HttpPost("activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto>> ActivateAccount([FromBody] ActivateAccountDto model)
        {
            try
            {
                await _authService.ActivateAccount(model.Code);
                return Ok(new ResponseDto { Message = "Account activated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Message = ex.Message });
            }
        }

        /// <summary>
        /// Resends account activation code
        /// </summary>
        /// <param name="model">Email address</param>
        /// <returns>Success message</returns>
        [HttpPost("resend-code")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto>> ResendActivationCode([FromBody] ResendActivationCodeDto model)
        {
            try
            {
                await _authService.ResendActivationCode(model.Email);
                return Ok(new ResponseDto { Message = "New activation code sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Message = ex.Message });
            }
        }

        /// <summary>
        /// Initiates password reset process
        /// </summary>
        /// <param name="model">Email address</param>
        /// <returns>Success message</returns>
        [HttpPost("request-password-reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto>> RequestPasswordReset([FromBody] RequestPasswordResetDto model)
        {
            try
            {
                await _authService.RequestPasswordReset(model.Email);
                return Ok(new ResponseDto { Message = "If the email exists, a reset link has been sent" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Message = ex.Message });
            }
        }

        /// <summary>
        /// Resets user password using reset token
        /// </summary>
        /// <param name="model">Reset token and new password</param>
        /// <returns>Success message</returns>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto>> ResetPassword([FromBody] ResetPasswordDto model)
        {
            try
            {
                await _authService.ResetPassword(model.Token, model.NewPassword);
                return Ok(new ResponseDto { Message = "Password has been reset successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto { Message = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _logHistoryService.UpdateLogoutTime(userId);
            return Ok();
        }

        private void SetRefreshToken(RefreshToken refreshToken)
        {
            ArgumentNullException.ThrowIfNull(refreshToken, nameof(refreshToken));

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }

    public class ResponseDto
    {
        public string Message { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}

