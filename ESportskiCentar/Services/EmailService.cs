using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ESportskiCentar.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PosaljiMail(string primaoc, string naslov, string sadrzaj)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:From"]));
            email.To.Add(MailboxAddress.Parse(primaoc));
            email.Subject = naslov;
            email.Body = new TextPart("html")
            {
                Text = sadrzaj
            };

            using var smtp = new SmtpClient();

            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            await smtp.ConnectAsync(smtpServer, port, SecureSocketOptions.SslOnConnect);

            var password = _configuration["EmailSettings:Password"]?.Replace(" ", "");

            await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], password);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}