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

            email.From.Add(MailboxAddress.Parse("esportskicentar@gmail.com"));
            email.To.Add(MailboxAddress.Parse(primaoc));
            email.Subject = naslov;
            email.Body = new TextPart("html")
            {
                Text = sadrzaj
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

           await smtp.AuthenticateAsync("esportskicentar@gmail.com", "ecwwiakmmcirecyl");

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}