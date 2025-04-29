using EclinicBackend.Dtos;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace EclinicBackend.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _emailServer;
        private readonly string _emailFrom;
        private readonly string _emailPassword;
        private readonly string _emailPort;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            // Try to get from environment variables first, then fall back to configuration
            _emailServer = _configuration.GetSection("EmailConfiguration:Server").Value
                ?? throw new InvalidOperationException("Email server configuration is missing");

            _emailFrom = _configuration.GetSection("EmailConfiguration:From").Value
                ?? throw new InvalidOperationException("Email from address is missing");

            _emailPassword = _configuration.GetSection("EmailConfiguration:Password").Value
                ?? throw new InvalidOperationException("Email password is missing");

            _emailPort = _configuration.GetSection("EmailConfiguration:Port").Value
                ?? throw new InvalidOperationException("Email port is missing");
        }

        public async Task<ServiceResponse<string>> SendEmail(SendEmailDto sendEmailDto)
        {
            var response = new ServiceResponse<string>();

            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_emailFrom));
                email.To.Add(MailboxAddress.Parse(sendEmailDto.ToEmail));
                email.Subject = sendEmailDto.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = sendEmailDto.Content };

                using var smtp = new SmtpClient();
                if (int.TryParse(_emailPort, out int port))
                {
                    await smtp.ConnectAsync(_emailServer, port, SecureSocketOptions.StartTls);
                }
                else
                {
                    throw new InvalidOperationException($"Email port '{_emailPort}' is not a valid integer");
                }
                await smtp.AuthenticateAsync(_emailFrom, _emailPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                response.Message = "Email sent successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
