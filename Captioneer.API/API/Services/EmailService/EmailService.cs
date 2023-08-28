using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

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
            this._config = config;
            this.emailHost = this._config.GetValue<string>("EmailInformation:EmailHost");
            this.emailName = this._config.GetValue<string>("EmailInformation:EmailName");
            this.emailUsername = this._config.GetValue<string>("EmailInformation:EmailUsername");
            this.emailPassword = this._config.GetValue<string>("EmailInformation:EmailPassword");
        }
        public void SendEmail(EmailViewModel req)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(this.emailName, this.emailUsername));
            email.To.Add(MailboxAddress.Parse(req.RecipientEmail));
            email.Subject = req.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = req.Body };

            using var smtp = new SmtpClient();

            smtp.Connect(this.emailHost, this.smtpPortNumber, SecureSocketOptions.StartTls);
            smtp.Authenticate(this.emailUsername, this.emailPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}