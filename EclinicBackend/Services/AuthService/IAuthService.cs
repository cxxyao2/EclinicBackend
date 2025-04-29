
using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.AuthService
{
    public interface IAuthService
    {
        Task Register(UserCreateDto user);
        Task<TokenPair> Login(string username, string password);
        Task<TokenPair> RefreshToken(string oldRefreshToken);
        Task<bool> ResendActivationCode(string email);
        Task<bool> ActivateAccount(string code);
        Task<bool> RequestPasswordReset(string email);
        Task<bool> ResetPassword(string token, string newPassword);
    }
}
