using EclinicBackend.Dtos;
using EclinicBackend.Models;

namespace EclinicBackend.Services.EmailService
{
    public interface IEmailService
    {
        Task<ServiceResponse<string>> SendEmail(SendEmailDto sendEmailDto);
    }
}