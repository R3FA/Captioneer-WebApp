using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Cryptography;
using API.Data;

namespace API.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        private string emailHost = string.Empty;
        private string emailName = string.Empty;
        private string emailUsername = string.Empty;
        private string emailPassword = string.Empty;
        private int smtpPortNumber = 587;

        public EmailService(IConfiguration config)
        {
            _config = config;
            this.emailHost = _config.GetValue<string>("EmailInformation:EmailHost");
            this.emailName = _config.GetValue<string>("EmailInformation:EmailName");
            this.emailUsername = _config.GetValue<string>("EmailInformation:EmailUsername");
            this.emailPassword = _config.GetValue<string>("EmailInformation:EmailPassword");
        }
        public async void TwoStepVerificationMail(EmailViewModel req)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(this.emailName, this.emailUsername));
            email.To.Add(MailboxAddress.Parse(req.RecipientEmail));
            email.Subject = req.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = req.Body };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(this.emailHost, this.smtpPortNumber, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(this.emailUsername, this.emailPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}