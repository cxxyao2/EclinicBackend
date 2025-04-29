using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

using EclinicBackend.Data;
using EclinicBackend.Dtos;
using EclinicBackend.Enums;
using EclinicBackend.Helpers;
using EclinicBackend.Models;
using EclinicBackend.Services.EmailService;

namespace EclinicBackend.Services.AuthService
{

    public class AuthService : IAuthService
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(ApplicationDbContext context, IConfiguration config, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenPair> RefreshToken(string oldRefreshToken)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == oldRefreshToken);
            if (user == null)
            {
                throw new Exception("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.UtcNow)
            {
                throw new Exception("Token expired.");
            }

            TokenPair tokenPair = await GenerateTokens(user);
            return tokenPair;
        }

        public async Task<TokenPair> Login(string email, string password)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            if (user == null || !PasswordHashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Email or password is incorrect");
            }

            if (!user.IsActivated)
            {
                throw new Exception("Please activate your account first");
            }

            TokenPair tokenPair = await GenerateTokens(user);
            tokenPair.user = user;

            return tokenPair;
        }


        public async Task Register(UserCreateDto model)
        {
            if (await _context.Users.AnyAsync(x => x.Email == model.Email))
            {
                throw new AppException("User with the email '{0}' already exists", model.Email);
            }

            User user = new User
            {
                UserName = model.UserName,
                Role = model.Role ?? UserRole.Nurse,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActivated = false,
                ActivationCode = Generate6DigitCode(),
                ActivationCodeExpires = DateTime.UtcNow.AddMinutes(15) // Code expires in 15 minutes
            };

            PasswordHashingHelper.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Send activation email
            await SendActivationEmail(user);
        }

        private string Generate6DigitCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task SendActivationEmail(User user)
        {
            var emailDto = new SendEmailDto
            {
                ToEmail = user.Email,
                Subject = "Your eClinic Activation Code",
                Content = $@"
                    <h2>Welcome to eClinic!</h2>
                    <p>Your activation code is: <strong>{user.ActivationCode}</strong></p>
                    <p>This code will expire in 15 minutes.</p>
                    <p>If you didn't create an account, please ignore this email.</p>"
            };

            var response = await _emailService.SendEmail(emailDto);
            if (!response.Success)
            {
                throw new Exception("Failed to send activation email");
            }
        }

        public async Task<bool> ActivateAccount(string code)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.ActivationCode == code &&
                u.ActivationCodeExpires > DateTime.UtcNow);

            if (user == null)
            {
                throw new Exception("Invalid or expired activation code");
            }

            user.IsActivated = true;
            user.ActivationCode = null;
            user.ActivationCodeExpires = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResendActivationCode(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Email == email &&
                !u.IsActivated);

            if (user == null)
            {
                throw new Exception("User not found or already activated");
            }

            user.ActivationCode = Generate6DigitCode();
            user.ActivationCodeExpires = DateTime.UtcNow.AddMinutes(15);
            await _context.SaveChangesAsync();

            await SendActivationEmail(user);
            return true;
        }

        private async Task<TokenPair> GenerateTokens(User user)
        {
            string? tokenSecret = _config["SECRETS_TOKEN"];
            if (string.IsNullOrEmpty(tokenSecret))
            {
                throw new Exception("Token secret is not set");
            }
            string token = TokenCreatingHelper.CreateToken(user, tokenSecret);
            RefreshToken newRefreshToken = TokenCreatingHelper.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            TokenPair tokenPair = new()
            { AccessToken = token, RefreshToken = newRefreshToken };

            return tokenPair;

        }

        public async Task<bool> RequestPasswordReset(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Generate reset token (valid for 1 hour)
            var resetToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);

            await _context.SaveChangesAsync();

            // Send reset email
            var resetLink = $"{GetClientOrigin()}/reset-password?token={resetToken}";
            var emailDto = new SendEmailDto
            {
                ToEmail = user.Email,
                Subject = "Password Reset Request",
                Content = $@"
                    <h2>Password Reset Request</h2>
                    <p>Click the link below to reset your password:</p>
                    <p><a href='{resetLink}'>Reset Password</a></p>
                    <p>This link will expire in 1 hour.</p>
                    <p>If you didn't request this, please ignore this email.</p>"
            };

            var response = await _emailService.SendEmail(emailDto);
            if (!response.Success)
            {
                throw new Exception("Failed to send reset email");
            }

            return true;
        }

        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.PasswordResetToken == token &&
                u.PasswordResetTokenExpires > DateTime.UtcNow);

            if (user == null)
            {
                throw new Exception("Invalid or expired reset token");
            }

            PasswordHashingHelper.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private string GetClientOrigin()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new InvalidOperationException("HttpContext is not available");

            // Try Origin header first
            var origin = httpContext.Request.Headers.Origin.ToString();

            // If Origin is not available, try Referer
            if (string.IsNullOrEmpty(origin))
            {
                var referer = httpContext.Request.Headers.Referer.ToString();
                if (!string.IsNullOrEmpty(referer))
                {
                    var uri = new Uri(referer);
                    origin = $"{uri.Scheme}://{uri.Host}";

                    // Only add port if it's non-standard and we're in development
                    if (!uri.IsDefaultPort &&
                        (uri.Port == 4200 || uri.Port == 3000 || uri.Port == 8080)) // Common development ports
                    {
                        origin += $":{uri.Port}";
                    }
                }
            }

            // Fallback to configuration if neither header is available
            if (string.IsNullOrEmpty(origin))
            {
                origin = _config["AppUrl"] ?? throw new InvalidOperationException("Could not determine client origin");
            }

            return origin.TrimEnd('/');
        }

    }

}
